using HrApi.Calculators;
using HrApi.DTOs.Reviews.PerformanceReview;
using HrApi.DTOs.Reviews.ReviewPeriods;
using HrApi.DTOs.Reviews.ReviewRubrics;
using HrApi.DTOs.Reviews.ReviewScores;
using HrApi.Enums.Review;
using HrApi.Exceptions;
using HrApi.Interfaces;
using HrApi.Models.Performance.Review;
using HrApi.Models.Performance.Rubric;
using HrApi.Models.Performance.Snapshots;
using HrApi.Models.Statics;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace HrApi.Data.Services;

public sealed class ReviewService : IReviewService
{
    private readonly HrDbContext _context;
    public ReviewService(HrDbContext context) => _context = context;

    private static PerformanceReviewResponseDto
    BuildPerformanceReviewResponse(
        PerformanceReview review,
        ReviewRubric rubric)
    {
        var scoreByCode = review.Scores
            .ToDictionary(
                x => x.CriterionCode,
                StringComparer.OrdinalIgnoreCase);

        var breakdown = rubric.Criteria
            .Select(criterion =>
            {
                scoreByCode.TryGetValue(
                    criterion.Code,
                    out var score);

                return new ReviewScoreBreakdownDto
                {
                    CriterionCode = criterion.Code,
                    Weight = criterion.Weight,
                    Score = score?.Score,
                    WeightedScore = score is null
                        ? 0
                        : score.Score * (criterion.Weight / 100m)
                };
            })
            .ToList();

        return new PerformanceReviewResponseDto
        {
            Id = review.Id,
            ReviewPeriodId = review.ReviewPeriodId,
            ReviewRubricId = review.ReviewRubricId,
            EmployeeId = review.EmployeeId,
            Status = review.Status,
            OverallScore = review.OverallScore,
            Breakdown = breakdown
        };
    }

    public async Task<ReviewRubric> CreateRubricVesionAsync(
        CreateReviewRubricRequest request,
        CancellationToken cancellationToken)
    {
        var lastVersion = await _context.ReviewRubrics
            .Where(x => x.Name == request.Name)
            .MaxAsync(
                x => (int?)x.Version,
                cancellationToken) ?? 0;

        var newVersion = lastVersion + 1;

        var rubric = ReviewRubric.Create(
            newVersion,
            request.Name);

        foreach (var criterion in request.Criteria)
        {
            if (!CriterionCodes.All.Contains(
                criterion.Code,
                StringComparer.OrdinalIgnoreCase))
            {
                throw new BusinessRuleException(
                    $"Unknown criterion code: {criterion.Code}");
            }

            rubric.AddCriterion(
                criterion.Code,
                criterion.Weight);
        }

        rubric.EnsureValidWeights();

        _context.ReviewRubrics.Add(rubric);

        await _context.SaveChangesAsync(
            cancellationToken);

        return rubric;
    }
    public async Task<ReviewPeriodResponseDto> CreateReviewPeriodAsync(
        CreateReviewPeriodRequest request,
        CancellationToken cancellationToken)
    {
        var rubric = await _context.ReviewRubrics
            .FirstOrDefaultAsync(
            x => x.Id == request.RubricId, cancellationToken);

        if (rubric is null)
            throw new NotFoundException(
                $"Rubric with id {request.RubricId} was not found.");

        var reviewPeriod = ReviewPeriod.Create(
            request.Title, request.StartsOn, request.EndsOn);

        reviewPeriod.SelectRubric(rubric.Id);

        await _context.ReviewPeriods
            .AddAsync(reviewPeriod, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return new ReviewPeriodResponseDto
        {
            Id = reviewPeriod.Id,
            Title = reviewPeriod.Title,
            StartsOn = reviewPeriod.StartsOn,
            EndsOn = reviewPeriod.EndsOn,
            IsClosed = reviewPeriod.IsClosed,
            Rubric = new RubricSummaryDto
            {
                Id = rubric.Id,
                Version = rubric.Version
            }
        };
    }
    public async Task<PerformanceReviewResponseDto> CreatePerformanceReviewAsync(
        CreatePerformanceReviewDto request,
        CancellationToken cancellationToken)
    {
        var employeeExists = await _context.Employees
            .AnyAsync(x => x.Id == request.EmployeeId, cancellationToken);

        if(!employeeExists)
            throw new NotFoundException(
            $"Employee with id {request.EmployeeId} was not found.");

        var reviewPeriod = await _context.ReviewPeriods
            .FirstOrDefaultAsync(
            x => x.Id == request.ReviewPeriodId, cancellationToken);

        if(reviewPeriod is null)
            throw new NotFoundException(
            $"Review period with id {request.ReviewPeriodId} was not found.");

        if(reviewPeriod.IsClosed)
            throw new BusinessRuleException(
            "Cannot create a review for a closed review period.");

        if (reviewPeriod.SelectedRubricId is null)
            throw new BusinessRuleException(
            "No rubric has been selected for this review period.");

        var rubric = await _context.ReviewRubrics
            .Include(x => x.Criteria)
            .FirstOrDefaultAsync(
            x => x.Id == reviewPeriod.SelectedRubricId,
            cancellationToken);

        if(rubric is null)
            throw new NotFoundException(
            "The selected rubric was not found.");

        var review = new PerformanceReview(
        reviewPeriod.Id,
        rubric.Id,
        request.EmployeeId);

        await _context.PerformanceReviews.AddAsync(review, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        var breakdown = rubric.Criteria
            .Select(x => new ReviewScoreBreakdownDto
            {
                CriterionCode = x.Code,
                Weight = x.Weight,
                Score = null,
                WeightedScore = 0
            })
            .ToList();

        return new PerformanceReviewResponseDto
        {
            Id = review.Id,
            ReviewPeriodId = review.ReviewPeriodId,
            ReviewRubricId = review.ReviewRubricId,
            EmployeeId = review.EmployeeId,
            Status = review.Status,
            OverallScore = null,
            Breakdown = breakdown
        };
    }
    public async Task<PerformanceReviewResponseDto>
    UpdatePerformanceReviewAsync(
        Guid id,
        UpdatePerformanceReviewDto request,
        CancellationToken cancellationToken)
    {
        var review = await _context.PerformanceReviews
        .Include(x => x.Scores)
        .FirstOrDefaultAsync(
            x => x.Id == id,
            cancellationToken);

        if (review is null)
        {
            throw new NotFoundException(
                $"Performance review with id {id} was not found.");
        }
        if (review.Status != ReviewStatus.Draft)
        {
            throw new ConflictException(
            "The review cannot be edited in its current status." +
            " INVALID_REVIEW_TRANSITION");
        }

        var rubric = await _context.ReviewRubrics
        .Include(x => x.Criteria)
        .FirstOrDefaultAsync(
            x => x.Id == review.ReviewRubricId,
            cancellationToken);

        if (rubric is null)
        {
            throw new NotFoundException(
                "The review rubric was not found.");
        }

        if (request.Scores.Count != rubric.Criteria.Count)
        {
            throw new BusinessRuleException(
                "All rubric criteria must be scored.");
        }

        var duplicateCodes = request.Scores
        .GroupBy(
            x => x.CriterionCode,
            StringComparer.OrdinalIgnoreCase)
        .Any(x => x.Count() > 1);

        if (duplicateCodes)
        {
            throw new BusinessRuleException(
                "Duplicate criterion codes are not allowed.");
        }

        foreach( var scoreRequest in request.Scores )
        {
            var criterion = rubric.Criteria
            .FirstOrDefault(x =>
                x.Code.Equals(
                    scoreRequest.CriterionCode,
                    StringComparison.OrdinalIgnoreCase));

            if (criterion is null)
            {
                throw new BusinessRuleException(
                    $"Criterion '{scoreRequest.CriterionCode}' " +
                    $"does not exist in this rubric.");
            }

            var existingScore = review.Scores
            .FirstOrDefault(x =>
                x.CriterionCode.Equals(
                    criterion.Code,
                    StringComparison.OrdinalIgnoreCase));

            if(existingScore is null)
            {
                review.Scores.Add(
                    new ReviewScore(
                    review.Id,
                    criterion.Code,
                    scoreRequest.Score,
                    scoreRequest.Comment));
            }
            else
            {
                existingScore.Update(
                    scoreRequest.Score,
                    scoreRequest.Comment);
            }
        }

        var overallScore = PerformanceScoreCalculator.CalculateOverall(
            review.Scores,
            rubric.Criteria);

        review.UpdateOverallScore(
            overallScore);

        try
        {
            await _context.SaveChangesAsync(
                cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConflictException(
                "The review was changed by another operation. " +
                "Reload the data and try again." +
                " CONCURRENCY_CONFLICT");
        }

        return BuildPerformanceReviewResponse(
            review, rubric);
    }
    public async Task<PerformanceReviewResponseDto> SubmitPerformanceReviewAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var review = await _context.PerformanceReviews
            .Include(x => x.Scores)
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (review is null)
        {
            throw new NotFoundException(
                $"Performance review with id {id} was not found.");
        }

        var reviewPeriod = await _context.ReviewPeriods
            .FirstOrDefaultAsync(
                x => x.Id == review.ReviewPeriodId,
                cancellationToken);

        if (reviewPeriod is null)
        {
            throw new NotFoundException(
                $"Review period with id {review.ReviewPeriodId} " +
                $"was not found.");
        }

        if (reviewPeriod.IsClosed)
        {
            throw new ConflictException(
                "The review period is closed." + 
                " REVIEW_PERIOD_CLOSED");
        }

        if (review.Status != ReviewStatus.Draft)
        {
            throw new ConflictException(
                "The review cannot be submitted in its current status." +
                " INVALID_REVIEW_TRANSITION");
        }

        var rubric = await _context.ReviewRubrics
            .Include(x => x.Criteria)
            .FirstOrDefaultAsync(
                x => x.Id == review.ReviewRubricId,
                cancellationToken);

        if (rubric is null)
        {
            throw new NotFoundException(
                $"Review rubric with id {review.ReviewRubricId} " +
                $"was not found.");
        }

        var missingCriteria = rubric.Criteria
        .Where(criterion =>
            !review.Scores.Any(score =>
                score.CriterionCode.Equals(
                    criterion.Code,
                    StringComparison.OrdinalIgnoreCase)))
        .ToList();

        if (missingCriteria.Count > 0)
        {
            throw new BadRequestException(
                "All rubric criteria must be scored.");
        }

        var overallScore =
            PerformanceScoreCalculator.CalculateOverall(
                review.Scores,
                rubric.Criteria);

        var snapshot = new PerformanceReviewSnapshot
        {
            RubricVersion = rubric.Version,
            Criteria = rubric.Criteria
            .Select(criterion =>
            {
                var score = review.Scores.First(
                    x => x.CriterionCode.Equals(
                        criterion.Code,
                        StringComparison.OrdinalIgnoreCase));

                return new SnapshotCriterion
                {
                    Code = criterion.Code,
                    Weight = criterion.Weight,
                    Score = score.Score
                };
            })
            .ToList(),
            OverallScore = overallScore
        };

        var snapshotJson = JsonSerializer.Serialize(snapshot);

        review.SetFinalResult(
            overallScore,
            snapshotJson);

        review.Submit(DateTimeOffset.UtcNow);

        try
        {
            await _context.SaveChangesAsync(
                cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConflictException(
                "The review was changed by another operation. " +
                "Reload the data and try again." +
                " CONCURRENCY_CONFLICT");
        }

        return BuildPerformanceReviewResponse(
            review, rubric);
    }
    public async Task<PerformanceReviewResponseDto>
    AcknowledgePerformanceReviewAsync(
        Guid id,
        string? employeeComment,
        CancellationToken cancellationToken)
    {
        var review = await _context.PerformanceReviews
            .Include(x => x.Scores)
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (review is null)
        {
            throw new NotFoundException(
                $"Performance review with id {id} was not found.");
        }

        var reviewPeriod = await _context.ReviewPeriods
            .FirstOrDefaultAsync(
                x => x.Id == review.ReviewPeriodId,
                cancellationToken);

        if (reviewPeriod is null)
        {
            throw new NotFoundException(
                $"Review period with id {review.ReviewPeriodId} " +
                $"was not found.");
        }

        if (reviewPeriod.IsClosed)
        {
            throw new ConflictException(
                "The review period is closed." +
                " REVIEW_PERIOD_CLOSED");
        }

        if (review.Status != ReviewStatus.Submitted)
        {
            throw new ConflictException(
                "The review cannot be acknowledged in its current status." +
                " INVALID_REVIEW_TRANSITION");
        }

        review.Acknowledge(
            employeeComment,
            DateTimeOffset.UtcNow);

        try
        {
            await _context.SaveChangesAsync(
                cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConflictException(
                "The review was changed by another operation. " +
                "Reload the data and try again." +
                " CONCURRENCY_CONFLICT");
        }

        var rubric = await _context.ReviewRubrics
            .Include(x => x.Criteria)
            .FirstAsync(
                x => x.Id == review.ReviewRubricId,
                cancellationToken);

        return BuildPerformanceReviewResponse(
            review, rubric);
    }

    public async Task<PerformanceReviewResponseDto> ClosePerformanceReviewAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var review = await _context.PerformanceReviews
       .Include(x => x.Scores)
       .FirstOrDefaultAsync(
           x => x.Id == id,
           cancellationToken);

        if (review is null)
        {
            throw new NotFoundException(
                $"Performance review with id {id} was not found.");
        }

        var reviewPeriod = await _context.ReviewPeriods
            .FirstOrDefaultAsync(
                x => x.Id == review.ReviewPeriodId,
                cancellationToken);

        if (reviewPeriod is null)
        {
            throw new NotFoundException(
                $"Review period with id {review.ReviewPeriodId} " +
                $"was not found.");
        }

        if (reviewPeriod.IsClosed)
        {
            throw new ConflictException(
                "The review period is closed." +
                " REVIEW_PERIOD_CLOSED");
        }

        if (review.Status != ReviewStatus.Acknowledged)
        {
            throw new ConflictException(
                "The review cannot be closed in its current status." +
                " INVALID_REVIEW_TRANSITION");
        }

        reviewPeriod.Close();
        review.Close();

        try
        {
            await _context.SaveChangesAsync(
                cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConflictException(
                "The review was changed by another operation. " +
                "Reload the data and try again." +
                " CONCURRENCY_CONFLICT");
        }

        var rubric = await _context.ReviewRubrics
            .Include(x => x.Criteria)
            .FirstAsync(
                x => x.Id == review.ReviewRubricId,
                cancellationToken);

        return BuildPerformanceReviewResponse(
            review,
            rubric);
    }
    public async Task<List<PerformanceReviewHistoryDto>> 
        GetEmployeePerformanceReviewsAsync(
            int employeeId,
            CancellationToken cancellationToken)
    {
        var employeeExists = await _context.Employees
        .AnyAsync(
            x => x.Id == employeeId,
            cancellationToken);

        if (!employeeExists)
        {
            throw new NotFoundException(
                $"Employee with id {employeeId} was not found.");
        }

        var history = await _context.PerformanceReviews
            .AsNoTracking()
            .Where(x => x.EmployeeId == employeeId)
            .OrderByDescending(x => x.ReviewPeriod.StartsOn)
            .Select(x => new PerformanceReviewHistoryDto
            {
                Id = x.Id,
                PeriodTitle = x.ReviewPeriod.Title,
                Status = x.Status.ToString(),
                OverallScore = x.OverallScore,
                RubricSnapshotJson = x.RubricSnapshotJson,
                SubmittedAt = x.SubmittedAt,
                AcknowledgedAt = x.AcknowledgedAt
            })
            .ToListAsync(cancellationToken);

        return history;
    }
}
