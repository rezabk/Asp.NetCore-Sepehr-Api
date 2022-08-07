using BusinessLogic.BusinessLogics.Lesson;
using DAL.DTO.Lesson;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalPorject.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonBl _bl;

        public LessonController(ILessonBl bl)
        {
            _bl = bl;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _bl.GetAllLessons();
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _bl.GetLessonById(id);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost]
        public async Task<IActionResult> create(CreateLessonDto dto)
        {
            var res = await _bl.CreateLesson(dto);
            return StatusCode(res.StatusCode, res);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> edit(UpdateLessonDto dto, int id)
        {
            var res = await _bl.UpdateLesson(dto, id);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPatch]
        public async Task<IActionResult> Availability(int id)
        {
            var res = await _bl.Availability(id);
            return StatusCode(res.StatusCode, res);
        }

        [HttpDelete("{lessonId}")]
        public async Task<IActionResult> Delete(int lessonId)
        {
            var res = await _bl.RemoveLesson(lessonId);
            return StatusCode(res.StatusCode, res);
        }


    }
}
