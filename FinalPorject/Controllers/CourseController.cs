using System.Runtime.CompilerServices;
using BusinessLogic.BusinessLogics.Course;
using BusinessLogic.Contracts;
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

        private readonly ICourseRepository _course;
        private readonly ICourseBl _bl;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CourseController(ICourseRepository course, ICourseBl bl, IWebHostEnvironment webHostEnvironment)
        {
            _course = course;
            _bl = bl;
            _webHostEnvironment = webHostEnvironment;
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

        [HttpPost("[action]")]

        public async Task<IActionResult> UploadCourseImage(int courseId, IFormFile image)
        {
            var res = await _bl.UploadImageCourse(courseId, image);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("{courseId}")]
        public async Task<IActionResult?> GetCourseImage(int courseId)
        {
            var image = _course.GetCourseById(courseId);

            string path = _webHostEnvironment.ContentRootPath + "\\Images\\";

            var filePath = path + image.ImageName;
            if (System.IO.File.Exists(filePath))
            {
                byte[] b = System.IO.File.ReadAllBytes(filePath);
                return File(b, "image/png");
            }
            return BadRequest();

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
