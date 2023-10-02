using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TrybeHotel.Dto;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("booking")]

    public class BookingController : Controller
    {
        private readonly IBookingRepository _repository;
        public BookingController(IBookingRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Client")]
        public IActionResult Add([FromBody] BookingDtoInsert bookingInsert)
        {
            try
            {
                var token = HttpContext.User.Identity as ClaimsIdentity;
                var email = token?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

                var booking = _repository.Add(bookingInsert, email!);

                return Created("", booking);

            }
            catch (Exception ex) when (ex.Message == "Guest quantity over room capacity")
            {
                return BadRequest(new { message = "Guest quantity over room capacity" });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpGet("{Bookingid}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize]
        public IActionResult GetBooking(int BookingId)
        {
            try
            {
                var token = HttpContext.User.Identity as ClaimsIdentity;

                var email = token?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

                var Booking = _repository.GetBooking(BookingId, email!);

                if (Booking == null) return NotFound(new { message = "Booking not found" });

                return Ok(Booking);
                
            }
            catch (Exception ex) when (ex.Message == "Booking not found")
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }
    }
}