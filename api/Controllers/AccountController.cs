using api.Dtos;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterUserDTO registerUserDTO)
        {
            try
            {
                var user = await _accountService.RegisterUser(registerUserDTO);
                return Ok(user);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao registrar usuário: {e.Message}");
                return StatusCode(500);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(string email, string password)
        {
            try {
                var result = await _accountService.LoginUser(email, password);
                return Ok(result);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }
    }
}