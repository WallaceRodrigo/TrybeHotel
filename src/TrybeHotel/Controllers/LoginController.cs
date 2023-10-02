using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using TrybeHotel.Services;
using Microsoft.AspNetCore.Authorization;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("login")]

    public class LoginController : Controller
    {

        private readonly IUserRepository _repository;
        public LoginController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginDto login)
        {
            try
            {
                var user = _repository.Login(login);

                var token = new TokenGenerator().Generate(user);

                return Ok(new { token });
            }
            catch (Exception ex) when (ex.Message == "Usuário não encontrado")
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex) when (ex.Message == "Incorrect e-mail or password")
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}