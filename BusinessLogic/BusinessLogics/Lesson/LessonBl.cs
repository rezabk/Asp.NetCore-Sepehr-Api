using BusinessLogic.Contracts;
using BusinessLogic.Utils;
using DAL;
using DAL.DTO.Lesson;
using DAL.Model;
using Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BusinessLogic.BusinessLogics.Lesson
{
    public class LessonBl : ILessonBl
    {

        private readonly Mapper _mapper;
        private readonly ShamasiCalendar _shamasi;
        private readonly ILessonRepository _lesson;
        private readonly ICourseRepository _course;
        private readonly Serilog.ILogger _logger = Log.Logger;
        public LessonBl(Mapper mapper, ShamasiCalendar shamasi, ILessonRepository lesson, ICourseRepository course)
        {
            _mapper = mapper;
            _shamasi = shamasi;
            _lesson = lesson;
            _course = course;
        }
        public async Task<StandardResult> GetAllLessons()
        {
            var res = await _lesson.GetAllLessons();
            if (res.Count == 0)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "هیچ درسی وجود ندارد" },
                    StatusCode = 403,
                    Success = false,
                };
                return err;
            }


            var listLessonDto = new List<ShowLessonDto>();
            foreach (var item in res)
            {
                var tempDto = await _mapper.MapAsync(item, new ShowLessonDto());
                listLessonDto.Add(tempDto);
            }

            var sr = new StandardResult<List<ShowLessonDto>>
            {
                Messages = new List<string> { "AllLessons Retrived" },
                Result = listLessonDto,
                StatusCode = 200,
                Success = true,
            };
            return sr;






        }

        public async Task<StandardResult> GetLessonById(int id)
        {
            var lesson = _lesson.GetLessonById(id);

            if (lesson is null)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "درسی با آیدی وارد شده یافت نشد" },
                    StatusCode = 404,
                    Success = false
                };
                _logger.Error("FinalProject : /GetLessonById success:false");
                return er;
            }

            var lessonDto = await _mapper.MapAsync(lesson, new ShowLessonDto());
            var sr = new StandardResult<ShowLessonDto>
            {
                Messages = new List<string> { "Lesson Retrived" },
                Result = lessonDto,
                StatusCode = 200,
                Success = true
            };
            _logger.Information("FinalProject : /GetLessonById success:true");
            return sr;

        }


        public async Task<StandardResult> CreateLesson(CreateLessonDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.LessonName))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "لطفا نام درس را وارد کنید" },
                    StatusCode = 404,
                    Success = false
                };
                _logger.Error("FinalProject : /CreateLesson success:false");
                return err;
            }
            if (string.IsNullOrWhiteSpace(dto.LessonTopics.ToString()))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "لطفا تاپیک درس را وارد کنید" },
                    StatusCode = 404,
                    Success = false
                };
                _logger.Error("FinalProject : /CreateLesson success:false");
                return err;
            }

            if (dto.LessonTopics.Count == 0)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "لطفا تاپیک درس را وارد کنید" },
                    StatusCode = 404,
                    Success = false
                };
                _logger.Error("FinalProject : /CreateLesson success:false");
                return err;
            }

            var newLesson = new LessonModel
            {
                LessonName = dto.LessonName,
                Description = dto.Description,
                LessonTopics = dto.LessonTopics.ToArray(),
                CreationDate = _shamasi.ToShamsi(DateTime.Now),
                IsActive = dto.IsActive
            };

            var checkLessonName =await _lesson.CheckLessonName(dto.LessonName);

            if (checkLessonName)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "درسی با این نام وجود دارد" },
                    StatusCode = 409,
                    Success = false,
                };
                _logger.Error("FinalProject : /CreateLesson success:false");
                return err;
            }


            await _lesson.AddLesson(newLesson);
            var createdLesson = await _mapper.MapAsync(newLesson, new ShowLessonDto());
            var sr = new StandardResult<ShowLessonDto>
            {
                Messages = new List<string> { "Lesson Created" },
                Result = createdLesson,
                StatusCode = 201,
                Success = true,
            };
            _logger.Information("FinalProject : /CreateLesson success:true");
            return sr;

        }

        public async Task<StandardResult> UpdateLesson(UpdateLessonDto dto, int id)
        {
            var lesson = _lesson.GetLessonById(id);
            if (lesson is null)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "درسی با آیدی وارد شده یافت نشد" },
                    StatusCode = 404,
                    Success = false
                };
                _logger.Error("FinalProject : /UpdateLesson success:false");
                return er;
            }
            if (string.IsNullOrWhiteSpace(dto.LessonName))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "لطفا نام درس را وارد کنید" },
                    StatusCode = 404,
                    Success = false
                };
                _logger.Error("FinalProject : /UpdateLesson success:false");
                return err;
            }

            if (string.IsNullOrWhiteSpace(dto.LessonTopics.ToString()))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "لطفا تاپیک درس را وارد کنید" },
                    StatusCode = 404,
                    Success = false
                };
                _logger.Error("FinalProject : /UpdateLesson success:false");
                return err;
            }
            if (dto.LessonTopics.Count == 0)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "لطفا تاپیک درس را وارد کنید" },
                    StatusCode = 404,
                    Success = false
                };
                _logger.Error("FinalProject : /UpdateLesson success:false");
                return err;
            }

            lesson.LessonName = dto.LessonName;
            lesson.LessonTopics = dto.LessonTopics.ToArray();
            lesson.Description = dto.Description;
            lesson.IsActive = dto.IsActive;

            _lesson.Save();
            var showUpdatedLesson = await _mapper.MapAsync(dto, new ShowUpdatedLessonDto());
            var sr = new StandardResult<ShowUpdatedLessonDto>
            {
                Messages = new List<string> { "Lesson Updated" },
                Result = showUpdatedLesson,
                StatusCode = 201,
                Success = true
            };
            _logger.Information("FinalProject : /UpdateLesson success:true");
            return sr;



        }

        public async Task<StandardResult> Availability(int id)
        {
            var lesson = _lesson.GetLessonById(id);
            if (lesson is null)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "درسی با آیدی مورد نظر پیدا نشد" },
                    StatusCode = 404,
                    Success = false

                };
                _logger.Error("FinalProject : /LessonAvailability success:false");
                return err;
            }

            if (lesson.IsActive)
            {
                lesson.IsActive = false;
                _lesson.Save();
                var sr = new StandardResult
                {
                    Messages = new List<string> { "درس غیر فعال شد" },
                    StatusCode = 201,
                    Success = true,
                };
                _logger.Information("FinalProject : /LessonAvailability success:true");
                return sr;
            }

            else
            {
                lesson.IsActive = true;
                _lesson.Save();
                var sr = new StandardResult
                {
                    Messages = new List<string> { "درس فعال شد" },
                    StatusCode = 201,
                    Success = true,
                };
                _logger.Information("FinalProject : /LessonAvailability success:true");
                return sr;
            }
        }

        public async Task<StandardResult> RemoveLesson(int lessonId)
        {
            var lesson = _lesson.GetLessonById(lessonId);
            if (lesson is null)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "درسی با آیدی داده شده وجود ندارد" },
                    StatusCode = 404,
                    Success = false,
                };
                _logger.Error("FinalProject : /RemoveLesson success:false");
                return er;
            }

            await _lesson.RemoveLesson(lessonId);
            await _course.CourseDeActiveOnLessonDeleting(lessonId);

            var sr = new StandardResult
            {
                Messages = new List<string> { "Lesson Removed" },
                StatusCode = 201,
                Success = true,
            };
            _logger.Information("FinalProject : /RemoveLesson success:true");
            return sr;

        }
    }
}
