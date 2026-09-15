using HrApi.Data.Services;
using HrApi.DTOs.Reviews.PerformanceReview;
using HrApi.Interfaces;
using HrApi.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrApi.ApiControllers;

[Route("api/performance-reviews")]
[ApiController]
public class PerformanceReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;
    public PerformanceReviewsController 
        (IReviewService reviewService) => _reviewService = reviewService;
    [HttpPost]
    public async Task<IActionResult> CreatePerformanceReview(
    [FromBody] CreatePerformanceReviewDto request,
    CancellationToken cancellationToken)
    {
        var review = await _reviewService
            .CreatePerformanceReviewAsync(
                request,
                cancellationToken);

        var response = new ApiResponse<PerformanceReviewResponseDto>
        {
            Success = true,
            Message = $"Performance Review for {request.EmployeeId} has been created.",
            Data = review
        };

        return StatusCode(
            StatusCodes.Status201Created,
            response);
    }
    [HttpPut("{Id:Guid}")]
    public async Task<IActionResult> UpdatePerformanceReview(
    Guid id,
    [FromBody] UpdatePerformanceReviewDto request,
    CancellationToken cancellationToken)
    {
        var review = await _reviewService
            .UpdatePerformanceReviewAsync(
                id,
                request,
                cancellationToken);

        var response = new ApiResponse<PerformanceReviewResponseDto>
        {
            Success = true,
            Message = $"Review with Id {id} has been updated",
            Data = review
        };

        return Ok(response);
    }
    [HttpPost("{Id:Guid}/submit")]
    public async Task<IActionResult> SubmitPerformanceReview(
    Guid id,
    CancellationToken cancellationToken)
    {
        var review = await _reviewService
            .SubmitPerformanceReviewAsync(
                id,
                cancellationToken);

        var response = new ApiResponse<PerformanceReviewResponseDto>
        {
            Success= true,
            Message = $"Review with Id {id} has been submitted.",
            Data = review
        };

        return Ok(response);
    }
    [HttpPost("{Id:Guid}/acknowledge")]
    public async Task<IActionResult> AcknowledgePerformanceReview(
    Guid id,
    [FromBody] AcknowledgePerformanceReviewDto request,
    CancellationToken cancellationToken)
    {
        var review = await _reviewService
            .AcknowledgePerformanceReviewAsync(
                id,
                request.EmployeeComment,
                cancellationToken);

        var response = new ApiResponse<PerformanceReviewResponseDto>
        {
            Success = true,
            Message = $"Review with Id {id} has been acknowledged.",
            Data = review
        };

        return Ok(response);
    }
    [HttpPost("{Id:Guid}/close")]
    public async Task<IActionResult> ClosePerformanceReview(
    Guid id,
    CancellationToken cancellationToken)
    {
        var review = await _reviewService
            .ClosePerformanceReviewAsync(
                id,
                cancellationToken);

        var response = new ApiResponse<PerformanceReviewResponseDto>
        {
            Success = true,
            Message = $"Review with Id {id} has been closed.",
            Data = review
        };

        return Ok(response);
    }
}
