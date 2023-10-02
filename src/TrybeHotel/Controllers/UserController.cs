using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("user")]

    public class UserController : Controller
    {
        private readonly IUserRepository _repository;
        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult GetUsers()
        {
            var users = _repository.GetUsers();

            return Ok(users);
        }

        [HttpPost]
        public IActionResult Add([FromBody] UserDtoInsert user)
        {
            if (user.Name == null || user.Email == null || user.Password == null)
            {
                return BadRequest(new { message = "Invalid user data" });
            }

            var existentUser = _repository.GetUserByEmail(user.Email);

            if (existentUser != null)
            {
                return Conflict(new { message = "User email already exists" });
            }

            var newUser = _repository.Add(user);

            return Created("", newUser);
        }
    }
}