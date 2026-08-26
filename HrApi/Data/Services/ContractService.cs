using Azure.Core;
using HrApi.ApiControllers;
using HrApi.DTOs.Contracts;
using HrApi.DTOs.Paging;
using HrApi.Enums.Contract;
using HrApi.Exceptions;
using HrApi.Interfaces;
using HrApi.Models;
using HrApi.Repositories;
using HrApi.Responses;
using HrApi.Specifications.Contracts;
using HrApi.Specifications.Employee.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HrApi.Data.Services;

public class ContractService : IContractService
{
    private readonly IReadRepository<EmployeeContract> _readRepository;
    private readonly HrDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public ContractService(
        IReadRepository<EmployeeContract> readRepository,
        HrDbContext context,
        ICurrentUserService currentUserService)
    {
        _readRepository = readRepository;
        _context = context;
        _currentUserService = currentUserService;
    }
    
    public async Task<int> CreateContractAsync(
    CreateContractRequest request,
    CancellationToken cancellationToken)
    {
        var exists = await _context.EmployeeContracts
            .AnyAsync(c => c.EmployeeId  == request.EmployeeId &&
            c.Status != ContractStatus.Cancelled,
            cancellationToken);

        if (exists)
        {
            throw new BusinessRuleException(
                "A Contract with this Employee ID already exists");
        }
        var contract = new EmployeeContract
        {
            EmployeeId = request.EmployeeId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            ProbationEndDate = request.ProbationEndDate,
            Status = ContractStatus.WaitingForSignature,
            ContractType = ContractType.WaitingForSignature,
            BaseSalary = request.BaseSalary,
            Currency = request.Currency
        };
        
        _context.EmployeeContracts.Add(contract);

        await _context.SaveChangesAsync(cancellationToken);

        return contract.Id;
    }
    public async Task<PagedResultDto<ContractListItemDto>> GetListAsync(
    ContractListRequest request,
    DateOnly today,
    CancellationToken cancellationToken)
    {
        await UpdateExpiredContractsAsync(
            today,
            cancellationToken);

        var specification = new 
             ContractListSpecification(request, today);

        var contractsQuery = await _readRepository
            .ListAsync(specification, cancellationToken);

        var countSpecification = new
            ContractCountSpecification(request, today);

        var totalItems = await _readRepository
            .CountAsync(countSpecification, cancellationToken);

        var items = contractsQuery
            .Select(x => new ContractListItemDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                EmployeeName = x.Employee.FullName,
                DepartmentName = 
                    x.Employee.Department.Name,
                ContractType = x.ContractType,
                Status = x.Status,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                BaseSalary = x.BaseSalary
            })
            .ToList();

        return new PagedResultDto<ContractListItemDto>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalItems,
            TotalPages = (int)Math.Ceiling(
                totalItems / (double)request.PageSize)
        };
    }
    public async Task<ApiResponse<ContractDetailsDto?>> GetByIdAsync(
    int id,
    CancellationToken cancellationToken)
    {
        var specification = new
            ContractDetailsSpecification(id);
        
        var contract = await _readRepository
            .FirstOrDefaultAsync(specification, cancellationToken);

        if(contract is null)
        {
            throw new NotFoundException("Contract Not Found");
        }

        UpdateExpiredStatus(
            contract,
            DateOnly.FromDateTime(DateTime.UtcNow));

        var dto = new ContractDetailsDto
        {
            Id = contract.Id,
            EmployeeId = contract.EmployeeId,
            ContractType = contract.ContractType,
            Status = contract.Status,
            StartDate = contract.StartDate,
            EndDate = contract.EndDate,
            BaseSalary = contract.BaseSalary
        };

        return new ApiResponse<ContractDetailsDto?>
        {
            Success = true,
            Message = "Contract retrieved.",
            Data = dto
        };
    }
    public async Task SubmitSignatureAsync(
    int contractId,
    SubmitSignatureRequest request,
    CancellationToken cancellationToken)
    {
        var specification = new
            FindContractByIdSpecification(contractId);

        var contract = await _readRepository
            .FirstOrDefaultAsync(specification, cancellationToken);

        if (contract is null)
        {
            throw new NotFoundException("Contract not found.");
        }

        contract.ContractType = request.ContractType;

        ChangeStatus(
            contract,
            ContractStatus.Signed,
            request.Notes);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task ActivateContractAsync(
        int contractId,
        ActivateContractRequest request,
        CancellationToken cancellationToken)
    {
        var specification = new
    FindContractByIdSpecification(contractId);

        var contract = await _readRepository
            .FirstOrDefaultAsync(specification, cancellationToken);

        if (contract is null)
        {
            throw new NotFoundException("Contract not found.");
        }
        if (contract.Status != ContractStatus.Signed)
        {
            throw new BusinessRuleException(
                "Contract must be Signed");
        }
        ChangeStatus(
            contract,
            ContractStatus.Active,
            request.Notes);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task CancelContractAsync(
        int contractId,
        CancelContractRequest request,
        CancellationToken cancellationToken)
    {
        var specification = new
            FindContractByIdSpecification(contractId);

        var contract = await _readRepository
            .FirstOrDefaultAsync(specification, cancellationToken);

        if (contract is null)
        {
            throw new NotFoundException("Contract not found.");
        }

        ChangeStatus(
            contract,
            ContractStatus.Cancelled,
            request.Reason.Trim());

        contract.Notes = request.Reason.Trim();

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RenewContractAsync(
        int contractId,
        string reason,
        CancellationToken cancellationToken)
    {
        var specification = new
            FindContractByIdSpecification(contractId);

        var contract = await _readRepository
            .FirstOrDefaultAsync(specification, cancellationToken);

        if (contract is null)
        {
            throw new NotFoundException("Contract not found.");
        }

        if(contract.Status != ContractStatus.Cancelled &&
            contract.Status != ContractStatus.Expired)
        {
            throw new BusinessRuleException(
                "Only Cancelled or Expired Contracts can be renewed");
        }

        ChangeStatus(
            contract,
            ContractStatus.WaitingForSignature,
            reason.Trim());

        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task CompleteContractAsync(
        int contractId,
        CompleteContractRequest request,
        CancellationToken cancellationToken)
    {
        var specification = new
            FindContractByIdSpecification(contractId);

        var contract = await _readRepository
            .FirstOrDefaultAsync(specification, cancellationToken);

        if (contract is null)
        {
            throw new NotFoundException("Contract not found.");
        }
        if(contract.Status != ContractStatus.Active)
        {
            throw new BusinessRuleException("Contract must be Active");
        }
        contract.ScoreRate = request.ScoreRate;
        contract.Notes = request.Notes.Trim();

        ChangeStatus(
            contract,
            ContractStatus.Completed,
            "Contract is done");

        await _context.SaveChangesAsync(cancellationToken);
    }

    private static bool CanTransition(
        ContractStatus from,
        ContractStatus to)
    {
        return (from, to) switch
        {
            (ContractStatus.WaitingForSignature,
            ContractStatus.Signed) => true,

            (ContractStatus.WaitingForSignature,
            ContractStatus.Cancelled) => true,

            (ContractStatus.Signed,
            ContractStatus.Active) => true,

            (ContractStatus.Signed,
            ContractStatus.Expired) => true,

            (ContractStatus.Active,
            ContractStatus.Cancelled) => true,

            (ContractStatus.Active,
            ContractStatus.Expired) => true,

            (ContractStatus.Active,
            ContractStatus.Completed) => true,

            (ContractStatus.Cancelled,
            ContractStatus.WaitingForSignature) => true,

            (ContractStatus.Expired,
             ContractStatus.WaitingForSignature) => true,

            _ => false
        };
    }
    private void ChangeStatus(
        EmployeeContract contract,
        ContractStatus targetStatus,
        string? reason)
    {
        var currentStatus = contract.Status;

        if(!CanTransition(currentStatus, targetStatus))
        {
            throw new BusinessRuleException(
            $"Transition from {currentStatus} " +
            $"to {targetStatus} is not allowed.");
        }

        contract.Status = targetStatus;

        contract.StateHistories.Add(new ContractStateHistory
        {
            ContractId = contract.Id,
            Employee = contract.Employee,
            FromState = currentStatus,
            ToState = targetStatus,
            Reason = reason,
            ChangedAtUtc = DateTime.UtcNow,
            ChangedByUserId = 
                _currentUserService.UserId
        });
    }
    private void UpdateExpiredStatus(
    EmployeeContract contract,
    DateOnly today)
    {
        if (today > contract.EndDate &&
            contract.Status != ContractStatus.Expired &&
            CanTransition(
                contract.Status,
                ContractStatus.Expired))
        {
            ChangeStatus(
                contract,
                ContractStatus.Expired,
                "Contract expired.");
        }
    }
    private async Task UpdateExpiredContractsAsync(
    DateOnly today,
    CancellationToken cancellationToken)
    {
        var contracts = await _context.EmployeeContracts
            .Where(c =>
                c.EndDate < today &&
                c.Status != ContractStatus.Expired &&
                c.Status != ContractStatus.Cancelled &&
                c.Status != ContractStatus.Completed)
            .ToListAsync(cancellationToken);

        foreach (var contract in contracts)
        {
            if (CanTransition(
                contract.Status,
                ContractStatus.Expired))
            {
                ChangeStatus(
                    contract,
                    ContractStatus.Expired,
                    "Contract expired.");
            }
        }

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}
