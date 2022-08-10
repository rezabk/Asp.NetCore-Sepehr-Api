using BusinessLogic.Contracts;
using BusinessLogic.Utils;
using DAL;
using DAL.DTO.Course;
using DAL.Model;
using DAL.Model.CourseModels;
using Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Serilog;
using Mapper = BusinessLogic.Utils.Mapper;

namespace BusinessLogic.BusinessLogics.Course
{
    public class CourseBl : ICourseBl
    {

        private readonly AcademyDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly Mapper _mapper;
        private readonly ShamasiCalendar _shamsi;
        private readonly ICourseUserRepository _courseUser;
        private readonly IAdminRepository _admin;
        private readonly IUserRepository _user;
        private readonly ICourseRepository _course;
        private ILessonRepository _lesson;
        private readonly Serilog.ILogger _logger = Log.Logger;

        public CourseBl(AcademyDbContext context, IWebHostEnvironment webHostEnvironment, Mapper mapper, ShamasiCalendar shamsi, ICourseUserRepository courseUser, IAdminRepository admin, IUserRepository user, ICourseRepository course, ILessonRepository lesson)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _shamsi = shamsi;
            _courseUser = courseUser;
            _admin = admin;
            _user = user;
            _course = course;
            _lesson = lesson;
        }
        public async Task<StandardResult> GetAllCourses()
        {
            var res = await _course.GetAllCourses();
            if (res.Count == 0)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "هیچ دوره ای وجود ندارد" },
                    StatusCode = 403,
                    Success = false,
                };
                _logger.Error("FinalProject : /GetAllCourses success:false");
                return err;
            }

            var listCoursesDto = new List<ShowCoursesDto>();

            foreach (var item in res)
            {
                var lessonDto = await _mapper.MapAsync(item.Lesson, new ShowCourseLessonDto());
                var teacherDto = await _mapper.MapAsync(item.Teacher, new ShowCourseUsersDto());
                teacherDto.Name = item.Teacher.FirstName + " " + item.Teacher.LastName;

                var courseStudents = await _courseUser.GetCourseStudents(item.Id, item.TeacherId);

                var students = new List<ShowCourseUsersDto>();
                foreach (var i in courseStudents)
                {
                    var student = _admin.GetUserById(i.UserId);
                    var studentDto = await _mapper.MapAsync(student, new ShowCourseUsersDto());
                    studentDto.Name = student.FirstName + " " + student.LastName;
                    students.Add(studentDto);
                }


                var tempDto = await _mapper.MapAsync(item, new ShowCoursesDto());

                tempDto.TeacherDetails = teacherDto;
                tempDto.LessonDetails = lessonDto;
                tempDto.StudentsDetails = students;

                listCoursesDto.Add(tempDto);
            }


            var sr = new StandardResult<List<ShowCoursesDto>>
            {
                Messages = new List<string> { "Courses Retrived" },
                Result = listCoursesDto,
                StatusCode = 200,
                Success = true,
            };
            _logger.Information("FinalProject : /GetAllCourses success:true");
            return sr;

        }

        public async Task<StandardResult> GetCourseById(int courseId)
        {
            var course = _course.GetCourseById(courseId);

            if (course is null)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "دوره ای با آیدی وارد شده یافت نشد" },
                    StatusCode = 404,
                    Success = false
                };
                _logger.Error("FinalProject : /GetCourseById success:false");
                return er;
            }

            var lessonDto = await _mapper.MapAsync(course.Lesson, new ShowCourseLessonDto());
            var teacherDto = await _mapper.MapAsync(course.Teacher, new ShowCourseUsersDto());
            teacherDto.Name = course.Teacher.FirstName + " " + course.Teacher.LastName;

            var courseStudents = await _courseUser.GetCourseStudents(course.Id, course.TeacherId);

            var students = new List<ShowCourseUsersDto>();
            foreach (var i in courseStudents)
            {
                var student = _admin.GetUserById(i.UserId);
                var studentDto = await _mapper.MapAsync(student, new ShowCourseUsersDto());
                studentDto.Name = student.FirstName + " " + student.LastName;
                students.Add(studentDto);
            }

            var tempDto = await _mapper.MapAsync(course, new ShowCoursesDto());

            tempDto.TeacherDetails = teacherDto;
            tempDto.LessonDetails = lessonDto;
            tempDto.StudentsDetails = students;

            var sr = new StandardResult<ShowCoursesDto>
            {
                Messages = new List<string> { "Courses Retrived" },
                Result = tempDto,
                StatusCode = 200,
                Success = true,
            };
            _logger.Information("FinalProject : /GetCourseById success:true");
            return sr;


        }


        public async Task<StandardResult> CreateCourse(CreateCourseDto dto, int id)
        {
            var teacher = _course.GetTeacher(id);
            if (teacher is null)
            {

                var er = new StandardResult
                {
                    Messages = new List<string> { "معملی با آیدی داده شده یافت نشد" },
                    StatusCode = 404,
                    Success = false,
                };
                _logger.Error("FinalProject : /CreateCourse success:false");
                return er;
            }

            var lesson = _course.GetLesson(dto.LessonId);
            if (lesson is null)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "درسی با آیدی داده شده یافت نشد" },
                    StatusCode = 404,
                    Success = false,
                };
                _logger.Error("FinalProject : /CreateCourse success:false");
                return err;
            }

            if (lesson.IsActive == false)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "درس مورد نظر غیر فعال است" },
                    StatusCode = 404,
                    Success = false,
                };
                _logger.Error("FinalProject : /CreateCourse success:false");
                return err;
            }

            if (string.IsNullOrWhiteSpace(dto.CourseTitle))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "لطفا نام دوره را وارد کنید" },
                    StatusCode = 404,
                    Success = false,
                };
                _logger.Error("FinalProject : /CreateCourse success:false");
                return err;
            }

            if (dto.Capacity < 10)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "ظرفیت دوره نمیتواند کمتر از 10 نفر باشد" },
                    StatusCode = 404,
                    Success = false,
                };
                _logger.Error("FinalProject : /CreateCourse success:false");
                return err;
            }


            var newCourse = new CourseModel
            {
                CourseTitle = dto.CourseTitle,
                Cost = dto.Cost,
                TeacherId = teacher.Id,
                Teacher = teacher,
                LessonId = lesson.Id,
                Lesson = lesson,
                StartDate = _shamsi.ToShamsi(dto.StartDate),
                EndDate = _shamsi.ToShamsi(dto.EndDate),
                CreationDate = _shamsi.ToShamsi(DateTime.Now),
                Capacity = dto.Capacity,

            };
            var res = await _course.CreateCourse(newCourse);

            var newCourseUser = new CourseUserModel
            {
                UserId = teacher.Id,
                CourseId = newCourse.Id,
                Course = newCourse,
                User = teacher,
            };
            await _courseUser.Add(newCourseUser);



            var teacherDto = await _mapper.MapAsync(teacher, new ShowCourseUsersDto());
            teacherDto.Name = teacher.FirstName + " " + teacher.LastName;
            var lessonDto = await _mapper.MapAsync(lesson, new ShowCourseLessonDto());
            var createdCourse = await _mapper.MapAsync(newCourse, new ShowCreatedCourseDto());
            createdCourse.TeacherDetails = teacherDto;
            createdCourse.LessonDetails = lessonDto;

            var sr = new StandardResult<ShowCreatedCourseDto>
            {
                Messages = new List<string> { "Course Created" },
                Result = createdCourse,
                StatusCode = 201,
                Success = true
            };
            _logger.Information("FinalProject : /CreateCourse success:true");
            return sr;
        }

        public async Task<StandardResult> UploadImageCourse(int courseId, IFormFile image)
        {
            var course = _course.GetCourseById(courseId);

            if (image != null)
            {
                FileInfo fi = new FileInfo(image.FileName);
                var newFileName = "Image_" + DateTime.Now.TimeOfDay.Milliseconds + fi.Extension;
                var path = Path.Combine("", _webHostEnvironment.ContentRootPath + "\\Images\\" + newFileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                course.ImagePath = path;
                course.ImageName = newFileName;
                _course.Save();

            }

            var sr = new StandardResult
            {
                Messages = new List<string> { "Image Uploaded" },
                StatusCode = 201,
                Success = true,
            };
            return sr;
        }

        public async Task<StandardResult> UpdateCourse(CreateCourseDto dto, int userId, int courseid)
        {
            var user = _user.GetUserById(userId);
            var course = _course.GetCourseById(courseid);
            var lesson = _lesson.GetLessonById(dto.LessonId);

            if (user.Id != course.TeacherId)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "شما معلم این دوره نیستید" },
                    StatusCode = 404,
                    Success = false,
                };
                _logger.Error("FinalProject : /UpdateCourse success:false");
                return er;
            }

            if (lesson is null)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "درسی با آیدی داده شده یافت نشد" },
                    StatusCode = 404,
                    Success = false,
                };
                _logger.Error("FinalProject : /UpdateCourse success:false");
                return er;
            }

            if (lesson.IsActive == false)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "درس مورد نظر غیر فعال است" },
                    StatusCode = 404,
                    Success = false,
                };
                _logger.Error("FinalProject : /UpdateCourse success:false");
                return err;
            }

            if (string.IsNullOrWhiteSpace(dto.CourseTitle))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "لطفا نام دوره را وارد کنید" },
                    StatusCode = 404,
                    Success = false,
                };
                _logger.Error("FinalProject : /UpdateCourse success:false");
                return err;
            }

            if (dto.Capacity < 10)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "ظرفیت دوره نمیتواند کمتر از 10 نفر باشد" },
                    StatusCode = 404,
                    Success = false,
                };
                _logger.Error("FinalProject : /UpdateCourse success:false");
                return err;
            }


            course.CourseTitle = dto.CourseTitle;
            course.Cost = dto.Cost;
            course.LessonId = dto.LessonId;
            course.Lesson = lesson;
            course.StartDate = _shamsi.ToShamsi(dto.StartDate);
            course.EndDate = _shamsi.ToShamsi(dto.EndDate);
            course.Capacity = dto.Capacity;

            _course.Save();

            var tempDto = await _mapper.MapAsync(dto, new ShowUpdatedCourseDto());

            var sr = new StandardResult<ShowUpdatedCourseDto>
            {
                Messages = new List<string> { "Course Updated" },
                Result = tempDto,
                StatusCode = 201,
                Success = true
            };
            _logger.Information("FinalProject : /UpdateCourse success:true");
            return sr;

        }

        public async Task<StandardResult> RemoveCourse(int userId, int courseid)
        {
            var teacher = _admin.GetUserById(userId);
            var course = _course.GetCourseById(courseid);

            if (course is null)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "دوره ای با آیدی داده شده یافت نشد" },
                    StatusCode = 404,
                    Success = false
                };
                _logger.Error("FinalProject : /RemoveCourse success:false");
                return er;
            }

            if (teacher.Id == course.TeacherId || teacher.Role == UserModel.UserRole.ADMIN)
            {
                await _course.RemoveCourse(course.Id);
                await _courseUser.RemoveCourse(course.Id);

                var sr = new StandardResult
                {
                    Messages = new List<string> { "Course Removed" },
                    StatusCode = 200,
                    Success = true,
                };
                _logger.Information("FinalProject : /RemoveCourse success:true");
                return sr;
            }

            var err = new StandardResult
            {
                Messages = new List<string> { "شما معلم این دوره نیستید" },
                StatusCode = 404,
                Success = false
            };
            _logger.Error("FinalProject : /RemoveCourse success:false");
            return err;
        }

        public async Task<StandardResult> Availability(int courseId, int userId)
        {
            var user = _admin.GetUserById(userId);
            var course = _course.GetCourseById(courseId);
            if (course is null)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "دوره ای با آیدی مورد نظر پیدا نشد" },
                    StatusCode = 404,
                    Success = false

                };
                _logger.Error("FinalProject : /Availability success:false");
                return err;
            }
            if (user.Role == UserModel.UserRole.TEACHER)
            {
                if (user.Id == course.TeacherId)
                {
                    if (course.IsActive)
                    {
                        course.IsActive = false;
                        _course.Save();
                        var sr = new StandardResult
                        {
                            Messages = new List<string> { "دوره غیر فعال شد" },
                            StatusCode = 201,
                            Success = true,
                        };
                        _logger.Information("FinalProject : /Availability success:true");
                        return sr;
                    }

                    if (course.IsActive == false)
                    {
                        course.IsActive = true;
                        _course.Save();
                        var sr = new StandardResult
                        {
                            Messages = new List<string> { "دوره فعال شد" },
                            StatusCode = 201,
                            Success = true,
                        };
                        _logger.Information("FinalProject : /Availability success:true");
                        return sr;
                    }
                }
            }
            if (user.Role == UserModel.UserRole.ADMIN)
            {
                if (course.IsActive)
                {
                    course.IsActive = false;
                    _course.Save();
                    var sr = new StandardResult
                    {
                        Messages = new List<string> { "دوره غیر فعال شد" },
                        StatusCode = 201,
                        Success = true,
                    };
                    _logger.Information("FinalProject : /Availability success:true");
                    return sr;
                }

                else
                {
                    course.IsActive = true;
                    _course.Save();
                    var sr = new StandardResult
                    {
                        Messages = new List<string> { "دوره فعال شد" },
                        StatusCode = 201,
                        Success = true,
                    };
                    _logger.Information("FinalProject : /Availability success:true");
                    return sr;
                }
            }
            var er = new StandardResult
            {
                Messages = new List<string> { "شما معلم این درس نیستید" },
                StatusCode = 404,
                Success = false
            };
            _logger.Error("FinalProject : /Availability success:true");
            return er;

        }
    }
}