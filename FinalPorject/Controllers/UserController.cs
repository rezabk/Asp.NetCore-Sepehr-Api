using BusinessLogic.BusinessLogics.User;
using BusinessLogic.Utils;
using DAL;
using DAL.DTO.User;
using DAL.Model;
using DAL.Model.CourseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalPorject.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]

    public class UserController : ControllerBase
    {
        private readonly IUserBl _bl;
        private readonly AcademyDbContext _context;

        public UserController(IUserBl bl, AcademyDbContext context)
        {
            _bl = bl;
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = int.Parse(HttpContext.User.Identity.Name);
            var res = await _bl.GetProfile(userId);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            var userId = int.Parse(HttpContext.User.Identity.Name);
            var res = await _bl.GetCourses(userId);
            return StatusCode(res.StatusCode, res);

        }

        [HttpPost("{id}")]
        public async Task<IActionResult> JoinCourse(int id)
        {
            var userId = int.Parse(HttpContext.User.Identity.Name);
            var res = await _bl.JoinCourse(id, userId);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(UpdateUserDto dto)
        {
            var userId = int.Parse(HttpContext.User.Identity.Name);
            var res = await _bl.UpdateUser(dto, userId);
            return StatusCode(res.StatusCode, res);

        }

        [HttpPost]
        public async Task<IActionResult> BecomeMaster(BecomeMasterDto dto)
        {
            var userId = int.Parse(HttpContext.User.Identity.Name);
            var res = await _bl.BecomeMaster(dto, userId);
            return StatusCode(res.StatusCode, res);
        }


    };

}