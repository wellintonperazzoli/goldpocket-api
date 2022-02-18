using GoldPocket.Models;
using GoldPocket.Models.DB;
using GoldPocket.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoldPocket.Controllers.API
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthController : ControllerBase
    {
        AuthService _service;
        ILogger<AuthController> _logger;
        public AuthController(AuthService service, ILogger<AuthController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            appUser appUser = new appUser
            {
                Email = user.Email,
                PasswordHash = user.Password
            };
            var result = await _service.Create(appUser);
            appUser.PasswordHash = null;
            return result.Succeeded ?
                Ok(appUser) :
                BadRequest(result.Errors);
        }

        [HttpPost]
        [Route("Token")]
        [AllowAnonymous]
        public async Task<IActionResult> Token([FromBody] User user)
        {
            try
            {
                return Ok(await _service.GenerateToken(new appUser
                {
                    Email = user.Email,
                    PasswordHash = user.Password
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
