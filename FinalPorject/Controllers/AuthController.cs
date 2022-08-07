using BusinessLogic.BusinessLogics.Auth;
using DAL.DTO.Auth;
using Microsoft.AspNetCore.Mvc;

namespace FinalPorject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthBl _bl;
         public AuthController(IAuthBl bl)
        {
            _bl = bl;
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var res = await _bl.Register(dto);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var res = await _bl.Login(dto);
            return StatusCode(res.StatusCode, res);

        }

    }
}
