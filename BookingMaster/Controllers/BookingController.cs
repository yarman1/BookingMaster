using BookingMaster.Constants;
using BookingMaster.Dtos.Bookings;
using BookingMaster.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookingMaster.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController(IBookingService bookingService) : ControllerBase
    {
        private readonly IBookingService _bookingService = bookingService;

        [HttpPost]
        [Authorize(Roles = Roles.Customer)]
        [ProducesResponseType(typeof(BookingCustomerResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBooking([FromBody] BookingCreateRequest request)
        {
            string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null || !int.TryParse(userIdString, out int customerId))
                return BadRequest("Invalid user ID");

            var result = await _bookingService.CreateBookingAsync(request, customerId);
            if (!result.Success)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetBookingByIdCustomer), new { bookingId = result.Data.Id }, result.Data);
        }

        [ProducesResponseType(typeof(BookingCustomerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("customer/{bookingId}")]
        [Authorize(Roles = Roles.Customer)]
        public async Task<IActionResult> GetBookingByIdCustomer(int bookingId)
        {
            string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null || !int.TryParse(userIdString, out int customerId))
                return BadRequest("Invalid user ID");

            var result = await _bookingService.GetBookingByIdCustomerAsync(bookingId, customerId);
            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        [HttpGet("customer")]
        [Authorize(Roles = Roles.Customer)]
        [ProducesResponseType(typeof(IEnumerable<BookingCustomerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBookingsByCustomer([FromQuery] BookingQuery query)
        {
            string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null || !int.TryParse(userIdString, out int customerId))
                return BadRequest("Invalid user ID");

            var result = await _bookingService.GetBookingsByCustomerAsync(query, customerId);
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }

        [HttpGet("owner/{bookingId}")]
        [Authorize(Roles = Roles.Owner)]
        [ProducesResponseType(typeof(BookingOwnerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBookingByIdOwner(int bookingId)
        {
            string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null || !int.TryParse(userIdString, out int ownerId))
                return BadRequest("Invalid user ID");

            var result = await _bookingService.GetBookingByIdOwnerAsync(bookingId, ownerId);
            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        [HttpGet("owner")]
        [Authorize(Roles = Roles.Owner)]
        [ProducesResponseType(typeof(IEnumerable<BookingOwnerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookingsByOwner([FromQuery] BookingQuery query)
        {
            string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null || !int.TryParse(userIdString, out int ownerId))
                return BadRequest("Invalid user ID");

            var result = await _bookingService.GetBookingsByOwnerAsync(query, ownerId);
            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        [HttpPut("status/{bookingId}")]
        [Authorize(Roles = Roles.Owner)]
        [ProducesResponseType(typeof(BookingOwnerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeBookingStatus(int bookingId, [FromBody] string newStatus)
        {
            string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null || !int.TryParse(userIdString, out int ownerId))
                return BadRequest("Invalid user ID");

            var result = await _bookingService.ChangeBookingStatus(bookingId, ownerId, newStatus);
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }
    }
}
