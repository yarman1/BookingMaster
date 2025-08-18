using BookingMaster.Constants;
using BookingMaster.Dtos.Feedbacks;
using BookingMaster.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookingMaster.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController(IFeedbackService feedbackService) : ControllerBase
    {
        private readonly IFeedbackService _feedbackService = feedbackService;

        [HttpPost]
        [Authorize(Roles = Roles.Customer)]
        [ProducesResponseType(typeof(FeedbackResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateFeedback([FromBody] FeedbackCreateRequest request)
        {
            string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null || !int.TryParse(userIdString, out int customerId))
                return BadRequest("Invalid user ID");

            var result = await _feedbackService.CreateFeedbackAsync(request, customerId);
            if (!result.Success)
                return BadRequest(result.Error);

            return StatusCode(201, result.Data);
        }

        [HttpPut("{feedbackId}")]
        [Authorize(Roles = Roles.Customer)]
        [ProducesResponseType(typeof(FeedbackResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateFeedback(int feedbackId, [FromBody] FeedbackUpdateRequest request)
        {
            string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null || !int.TryParse(userIdString, out int customerId))
                return BadRequest("Invalid user ID");

            var result = await _feedbackService.UpdateFeedbackAsync(feedbackId, request, customerId);
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }

        [HttpDelete("{feedbackId}")]
        [Authorize(Roles = Roles.Customer)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteFeedback(int feedbackId)
        {
            string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null || !int.TryParse(userIdString, out int customerId))
                return BadRequest("Invalid user ID");

            var result = await _feedbackService.DeleteFeedbackAsync(feedbackId, customerId);
            if (!result.Success)
                return BadRequest(result.Error);

            return NoContent();
        }

        [HttpGet("accommodation/{accommodationId}")]
        [ProducesResponseType(typeof(IEnumerable<FeedbackResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFeedbacksByAccommodation(int accommodationId)
        {
            var result = await _feedbackService.GetFeedbacksByAccommodationAsync(accommodationId);
            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Data);
        }
    }
}
