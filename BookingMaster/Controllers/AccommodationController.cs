using BookingMaster.Constants;
using BookingMaster.Dtos.Accommodation;
using BookingMaster.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookingMaster.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccommodationController(IAccommodationService accommodationService) : ControllerBase
    {
        private readonly IAccommodationService accommodationService = accommodationService;

        [HttpPost]
        [Authorize(Roles = Roles.Owner)]
        [ProducesResponseType(typeof(AccommodationResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAccommodation([FromBody] AccommodationCreateRequest request)
        {
            string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdString == null || !int.TryParse(userIdString, out int ownerId))
            {
                return BadRequest("Invalid user ID");
            }
            var result = await accommodationService.CreateAccommodationAsync(request, ownerId);
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }

            return CreatedAtAction(nameof(GetAccommodationById), new { id = result.Data.Id }, result.Data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Owner)]
        [ProducesResponseType(typeof(AccommodationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAccommodation(int id, [FromBody] AccommodationUpdateRequest request)
        {
            string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null || !int.TryParse(userIdString, out int ownerId))
            {
                return BadRequest("Invalid user ID");
            }
            var result = await accommodationService.UpdateAccommodationAsync(id, request, ownerId);
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }

            return CreatedAtAction(nameof(GetAccommodationById), new { id = result.Data.Id }, result.Data);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{Roles.Owner}, {Roles.Customer}")]
        [ProducesResponseType(typeof(AccommodationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAccommodationById(int id)
        {
            var result = await accommodationService.GetAccommodationByIdAsync(id);
            if (!result.Success)
            {
                return NotFound(result.Error);
            }
            return Ok(result.Data);
        }

        [HttpGet("search")]
        [Authorize(Roles = Roles.Customer)]
        [ProducesResponseType(typeof(IEnumerable<AccommodationSearchResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchAccommodations([FromQuery] AccommodationQuery query)
        {
            var result = await accommodationService.SearchAccommodationsAsync(query);
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Data);
        }

        [HttpGet("owner")]
        [Authorize(Roles = Roles.Owner)]
        [ProducesResponseType(typeof(IEnumerable<AccommodationSearchResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOwnerAccommodations()
        {
            string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null || !int.TryParse(userIdString, out int ownerId))
            {
                return BadRequest("Invalid user ID");
            }
            var result = await accommodationService.GetOwnerAccomodationsAsync(ownerId);
            if (!result.Success)
            {
                return NotFound(result.Error);
            }
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Owner)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAccommodation(int id)
        {
            string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null || !int.TryParse(userIdString, out int ownerId))
            {
                return BadRequest("Invalid user ID");
            }
            var result = await accommodationService.DeleteAccommodationAsync(id, ownerId);
            if (!result.Success)
            {
                return NotFound(result.Error);
            }
            return NoContent();
        }
    }
}
