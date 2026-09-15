using HrApi.DTOs.Reviews.ReviewRubrics;
using HrApi.Interfaces;
using HrApi.Models.Performance.Rubric;
using HrApi.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrApi.ApiControllers;

[Route("api/review-rubrics")]
[ApiController]
public class ReviewRubricsController : ControllerBase
{
    private readonly IReviewService _reviewService;
    public ReviewRubricsController(IReviewService reviewService) 
        => _reviewService = reviewService;
    
    [HttpPost]
    public async Task<ActionResult<ApiResponse<ReviewRubric>>>
        CreateRubricVesionAsync(
        [FromBody]CreateReviewRubricRequest request,
        CancellationToken cancellationToken)
    {
        var rubric = await _reviewService
            .CreateRubricVesionAsync(request, cancellationToken);

        var response = new ApiResponse<ReviewRubric>
        {
            Success = true,
            Message = "Review rubric created successfully",
            Data = rubric
        };

        return StatusCode(
            StatusCodes.Status201Created,
            response);
    }
}