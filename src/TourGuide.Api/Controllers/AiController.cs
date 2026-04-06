using Microsoft.AspNetCore.Mvc;
using TouristGuide.Api.Services;
using TouristGuide.Shared.DTOs;

namespace TouristGuide.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AiController : ControllerBase
{
    private readonly GroqService _groqService;
    private readonly ILogger<AiController> _logger;

    public AiController(GroqService groqService, ILogger<AiController> logger)
    {
        _groqService = groqService;
        _logger = logger;
    }

    /// <summary>
    /// Get AI-powered tour recommendation based on user preferences and history
    /// </summary>
    [HttpPost("recommend")]
    public async Task<ActionResult<AiRecommendResponse>> GetRecommendation([FromBody] AiRecommendRequest request)
    {
        _logger.LogInformation("AI recommendation request: Interests={Interests}, Budget={Budget}, Age={Age}, Health={Health}, SessionId={SessionId}",
            request.Interests, request.Budget, request.Age, request.HealthCondition, request.SessionId);

        var result = await _groqService.GetRecommendationAsync(request);

        if (result.Success)
        {
            _logger.LogInformation("AI recommendation success: Tours={Tours}", string.Join(",", result.RecommendedTourIds));
        }
        else
        {
            _logger.LogWarning("AI recommendation failed: {Error}", result.Error);
        }

        return Ok(result);
    }

    /// <summary>
    /// Health check for AI service
    /// </summary>
    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        return Ok(new { status = "ok", service = "AI Recommendation" });
    }
}
