using HrApi.DTOs.Reviews.ReviewPeriods;
using HrApi.Interfaces;
using HrApi.Models.Performance.Review;
using HrApi.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrApi.ApiControllers;

[Route("api/review-periods")]
[ApiController]
public class ReviewPeriodsController : ControllerBase
{
    private readonly IReviewService _reviewService;
    public ReviewPeriodsController(IReviewService reviewService)
        => _reviewService = reviewService;

    [HttpPost]
    public async Task<IActionResult> CreateReviewPeriod(
        CreateReviewPeriodRequest request,
        CancellationToken cancellationToken)
    {
        var reviewPeriod = await _reviewService
            .CreateReviewPeriodAsync(request, cancellationToken);

        var response = new ApiResponse<ReviewPeriodResponseDto>
        {
            Success = true,
            Message = $"Review period {reviewPeriod.Id} Created",
            Data = reviewPeriod
        };

        return StatusCode(
            StatusCodes.Status201Created,
            response);
    }
}
