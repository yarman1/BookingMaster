using BookingMaster.Constants;
using BookingMaster.Dtos.AccommodationProposal;
using BookingMaster.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookingMaster.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccommodationProposalController(IAccommodationProposalService accommodationProposalService) : ControllerBase
    {
        private readonly IAccommodationProposalService accommodationProposalService = accommodationProposalService;

        [HttpPost]
        [Authorize(Roles = Roles.Owner)]
        [ProducesResponseType(typeof(AccommodationProposalOwnerResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAccommodationProposal([FromBody] AccommodationProposalCreateRequest request)
        {
            string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null || !int.TryParse(userIdString, out int ownerId))
                return BadRequest("Invalid user ID");

            var result = await accommodationProposalService.CreateAccommodationProposalAsync(request, ownerId);
            if (!result.Success)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetAccommodationProposalByIdOwner), new { id = result.Data.Id }, result.Data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Owner)]
        [ProducesResponseType(typeof(AccommodationProposalOwnerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAccommodationProposal(int id, [FromBody] AccommodationProposalUpdateRequest request)
        {
            string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null || !int.TryParse(userIdString, out int ownerId))
                return BadRequest("Invalid user ID");

            var result = await accommodationProposalService.UpdateAccommodationProposalAsync(id, request, ownerId);
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Owner)]
        [ProducesResponseType(typeof(AccommodationProposalOwnerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAccommodationProposalByIdOwner(int id)
        {
            string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null || !int.TryParse(userIdString, out int ownerId))
                return BadRequest("Invalid user ID");

            var result = await accommodationProposalService.GetAccommodarionProposalByIdOwnerAsync(id, ownerId);
            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        [HttpPost("customer/available")]
        [Authorize(Roles = Roles.Customer)]
        [ProducesResponseType(typeof(IEnumerable<AccommodationProposalResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAvailableProposals([FromBody] AccommodationProposalAvailabilityRequest request)
        {
            var result = await accommodationProposalService.GetAccommodationProposalsCustomerAsync(request);
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }

        [HttpGet("owner/{accommodationId}")]
        [Authorize(Roles = Roles.Owner)]
        [ProducesResponseType(typeof(IEnumerable<AccommodationProposalOwnerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOwnerProposals(int accommodationId)
        {
            string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null || !int.TryParse(userIdString, out int ownerId))
                return BadRequest("Invalid user ID");

            var result = await accommodationProposalService.GetAccommodationProposalsOwnerAsync(accommodationId, ownerId);
            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Owner)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAccommodationProposal(int id)
        {
            string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null || !int.TryParse(userIdString, out int ownerId))
                return BadRequest("Invalid user ID");

            var result = await accommodationProposalService.DeleteAccommodationProposalAsync(id, ownerId);
            if (!result.Success)
                return BadRequest(result.Error);

            return NoContent();
        }
    }
}
