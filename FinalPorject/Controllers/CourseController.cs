using BusinessLogic.BusinessLogics.Course;
using DAL;
using DAL.DTO.Course;
using DAL.Model.CourseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalPorject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
  
    public class CourseController : ControllerBase
    {
        private readonly ICourseBl _bl;

        public CourseController(ICourseBl bl, AcademyDbContext context)
        {
            _bl = bl;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _bl.GetAllCourses();
            return StatusCode(res.StatusCode, res);
        }

        [Authorize]
        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetById(int courseId)
        {
            var res = await _bl.GetCourseById(courseId);
            return StatusCode(res.StatusCode, res);
        }


        [Authorize(Roles = "TEACHER")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseDto dto)
        {
            var userId = int.Parse(HttpContext.User.Identity.Name);
            var res = await _bl.CreateCourse(dto, userId);
            return StatusCode(res.StatusCode, res);

        }

        [Authorize(Roles = "TEACHER")]
        [HttpPatch("{courseid}")]
        public async Task<IActionResult> edit(UpdateCourseDto dto, int courseid)
        {
            var userId = int.Parse(HttpContext.User.Identity.Name);
            var res = await _bl.UpdateCourse(dto, userId, courseid);
            return StatusCode(res.StatusCode, res);
        }

        [Authorize(Roles = "TEACHER,ADMIN")]
        [HttpDelete("{courseid}")]
        public async Task<IActionResult> Delete(int courseid)
        {
            var userId = int.Parse(HttpContext.User.Identity.Name);
            var res = await _bl.RemoveCourse(userId, courseid);
            return StatusCode(res.StatusCode, res);
        }



        [Authorize(Roles = "TEACHER,ADMIN")]
        [HttpPatch]
        public async Task<IActionResult> Availability(int courseId)
        {
            var userId = int.Parse(HttpContext.User.Identity.Name);
            var res = await _bl.Availability(courseId, userId);
            return StatusCode(res.StatusCode, res);
        }


    }
}
