using AutoMapper.Configuration.Conventions;
using DAL.Model;
using BusinessLogic.Contracts;
using BusinessLogic.Utils;
using DAL;
using DAL.DTO;
using DAL.DTO.Admin;
using DAL.DTO.Course;
using DAL.Model.CourseModels;
using Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BusinessLogic.BusinessLogics
{
    public class AdminBl : IAdminBl
    {

        private readonly AcademyDbContext _context;
        private readonly Mapper _mapper;
        private readonly HashPassword _hash;
        private readonly IAdminRepository _repo;
        private readonly IBecomeMasterRepository _master;
        private readonly ICourseUserRepository _courseUser;
        private readonly IUserRepository _user;
        private readonly ICourseRepository _course;
        private readonly ILessonRepository _lesson;
        private readonly ShamasiCalendar _shamsi;
        private readonly ISettingRepository _setting;
        private readonly Serilog.ILogger _logger = Log.Logger;

        public AdminBl(AcademyDbContext context, Mapper mapper, HashPassword hash, IAdminRepository repo, IBecomeMasterRepository master, ICourseUserRepository courseUser, IUserRepository user, ICourseRepository course, ILessonRepository lesson, ShamasiCalendar shamsi, ISettingRepository setting)
        {
            _context = context;
            _mapper = mapper;
            _hash = hash;
            _repo = repo;
            _master = master;
            _courseUser = courseUser;
            _user = user;
            _course = course;
            _lesson = lesson;
            _shamsi = shamsi;
            _setting = setting;
        }



        public async Task<StandardResult> GetAllUsers()
        {
            var res = await _repo.GetAllUsers();
            if (res.Count == 0)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "هیچ کاربری وجود ندارد" },
                    StatusCode = 403,
                    Success = false,
                };
                _logger.Information("Class-ASP : /signin success:true");
                return err;
            }

            var listUserDto = new List<AdminShowUserDto>();
            foreach (var item in res)
            {
                var tempDto = await _mapper.MapAsync(item, new AdminShowUserDto());
                listUserDto.Add(tempDto);
            }
            var sr = new StandardResult<List<AdminShowUserDto>>
            {
                Messages = new List<string> { "AllUsers Retrived" },
                Result = listUserDto,
                StatusCode = 200,
                Success = true
            };
            return sr;

        }
        public async Task<StandardResult> GetUserById(int id)
        {

            var user = _repo.GetUserById(id);

            if (user is null)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "کاربری با آیدی وارد شده یافت نشد" },
                    StatusCode = 404,
                    Success = false
                };
                _logger.Information("Class-ASP : /GetUserById success:false");
                return er;
            }

            var userDto = await _mapper.MapAsync(user, new AdminShowUserDto());
            var sr = new StandardResult<AdminShowUserDto>
            {
                Messages = new List<string> { "UserModel Retrived" },
                Result = userDto,
                StatusCode = 200,
                Success = true
            };
            return sr;

        }

        public async Task<StandardResult> Availablity(int id)
        {
            var user = _repo.GetUserById(id);
            if (user is null)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "کاربری با آیدی مورد نظر پیدا نشد" },
                    StatusCode = 404,
                    Success = false

                };
                return err;
            }

            if (user.IsActive)
            {
                user.IsActive = false;
                _repo.Save();
                var sr = new StandardResult
                {
                    Messages = new List<string> { "کاربر غیر فعال شد" },
                    StatusCode = 201,
                    Success = true,
                };
                return sr;
            }

            else
            {
                user.IsActive = true;
                _repo.Save();
                var sr = new StandardResult
                {
                    Messages = new List<string> { "کاربر فعال شد" },
                    StatusCode = 201,
                    Success = true,
                };
                return sr;
            }


        }

        public async Task<StandardResult> GetAllBecomeMasterRequests()
        {
            var requests = await _master.GetAllRequests();
            if (requests.Count == 0)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "هیچ درخواستی یافت نشد" },
                    StatusCode = 404,
                    Success = false
                };
                return er;
            }

            var showRequestsList = new List<ShowBecomeMasterRequests>();

            foreach (var item in requests)
            {
                var tempDto = await _mapper.MapAsync(item, new ShowBecomeMasterRequests());
                showRequestsList.Add(tempDto);
            }

            var sr = new StandardResult<List<ShowBecomeMasterRequests>>
            {
                Messages = new List<string> { "Requests Retrived" },
                Result = showRequestsList,
                StatusCode = 200,
                Success = true,
            };
            return sr;
        }

        public async Task<StandardResult> GetBecomeMasterRequestById(int id)
        {
            var request = _master.GetRequestById(id);
            if (request is null)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "درخواستی با آیدی داده شده یافت نشد" },
                    StatusCode = 404,
                    Success = false,
                };
                return err;
            }

            var requestDto = await _mapper.MapAsync(request, new ShowBecomeMasterRequests());
            var sr = new StandardResult<ShowBecomeMasterRequests>
            {
                Messages = new List<string> { "UserModel Retrived" },
                Result = requestDto,
                StatusCode = 200,
                Success = true
            };
            return sr;


        }

        public async Task<StandardResult> ApproveMasterById(int id)
        {
            var request = _master.GetRequestById(id);
            if (request is null)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "درخواستی با آیدی داده شده یافت نشد" },
                    StatusCode = 404,
                    Success = false,
                };
                return err;
            }

            request.Approvement = true;

            var user = _repo.GetUserById(request.UserId);
            if (user.Role == UserModel.UserRole.TEACHER)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "کاربر هم اکنون معلم است" },
                    StatusCode = 404,
                    Success = false,
                };
                return er;
            }
            if (user.Role == UserModel.UserRole.ADMIN)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "کاربر ادمین است و نمیتواند به معلم تغییر وضعیت بدهد" },
                    StatusCode = 404,
                    Success = false,
                };
                return er;
            }

            user.Role = UserModel.UserRole.TEACHER;
            _repo.Save();

            var sr = new StandardResult
            {
                Messages = new List<string> { "درخواست کاربر برای معلم شدن تایید شد" },
                StatusCode = 201,
                Success = true,
            };
            return sr;
        }

        public async Task<StandardResult> CheckCreateUser(AdminCreateUserDto dto)
        {

            if (string.IsNullOrWhiteSpace(dto.FirstName))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "لطفا نام کاربر را وارد کنید" },
                    StatusCode = 404,
                    Success = false
                };
                return err;
            }
            if (string.IsNullOrWhiteSpace(dto.LastName))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "لطفا نام خوانوادگی کاربر را وارد کنید" },
                    StatusCode = 404,
                    Success = false
                };
                return err;
            }
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "لطفا ایمیل کاربر را وارد کنید" },
                    StatusCode = 404,
                    Success = false
                };
                return err;
            }
            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "لطفا رمز عبور کاربر را وارد کنید" },
                    StatusCode = 404,
                    Success = false
                };
                return err;
            }
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "لطفا شماره موبایل کاربر را وارد کنید" },
                    StatusCode = 404,
                    Success = false
                };
                return err;
            }

            if (await _context.Users.AnyAsync(x => x.Email == dto.Email))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "ایمیل وارد شده در سیستم موجود است" },
                    StatusCode = 409,
                    Success = false
                };
                return err;
            }
            if (await _context.Users.AnyAsync(x => x.PhoneNumber == dto.PhoneNumber))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "شماره موبایل وارد شده در سیستم موجود است" },
                    StatusCode = 409,
                    Success = false
                };
                return err;
            }


            var newUser = new UserModel
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Password = _hash.GetHash(dto.Password),
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Role = dto.Role,
                Address = dto.Address,
                IsActive = true,
                IsDeleted = false,
            };
            await _repo.AddUser(newUser);
            var createdUser = await _mapper.MapAsync(newUser, new AdminShowUserDto());
            var sr = new StandardResult<AdminShowUserDto>
            {
                Messages = new List<string> { "User Created" },
                Result = createdUser,
                StatusCode = 201,
                Success = true
            };
            return sr;


        }

        public async Task<StandardResult> CheckUpdateUser(AdminUpdateUserDto dto, int id)
        {
            var user = _repo.GetUserById(id);

            if (user is null)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "کاربری با آیدی وارد شده یافت نشد" },
                    StatusCode = 404,
                    Success = false
                };
                return er;
            }

            if (string.IsNullOrWhiteSpace(dto.FirstName))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "لطفا نام کاربر را وارد کنید" },
                    StatusCode = 404,
                    Success = false
                };
                return err;
            }
            if (string.IsNullOrWhiteSpace(dto.LastName))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "لطفا نام خوانوادگی کاربر را وارد کنید" },
                    StatusCode = 404,
                    Success = false
                };
                return err;
            }
            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "لطفا رمز عبور کاربر را وارد کنید" },
                    StatusCode = 404,
                    Success = false
                };
                return err;
            }

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Password = _hash.GetHash(dto.Password);
            user.PhoneNumber = dto.PhoneNumber;
            user.Address = dto.Address;
            user.Role = dto.Role;
            user.IsActive = dto.IsActive;

            if (await _context.Users.AnyAsync(x => x.PhoneNumber == dto.PhoneNumber && x.Id != user.Id))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "شماره موبایل وارد شده در سیستم وجود دارد" },
                    StatusCode = 409,
                    Success = false,
                };
                return err;
            }

            _repo.Save();
            var showUpdatedUser = await _mapper.MapAsync(dto, new AdminShowUpdatedUserDto());
            var sr = new StandardResult<AdminShowUpdatedUserDto>
            {
                Messages = new List<string> { "User Updated" },
                Result = showUpdatedUser,
                StatusCode = 201,
                Success = true
            };

            return sr;
        }

        public async Task<StandardResult> CheckRemoveUser(int id)
        {
            var user = _repo.GetUserById(id);

            if (user is null)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "کاربری با آیدی وارد شده یافت نشد" },
                    StatusCode = 404,
                    Success = false
                };
                return er;
            }

            await _repo.RemoveUser(id);
            await _courseUser.RemoveUser(id);

            var sr = new StandardResult
            {
                Messages = new List<string> { "User Removed" },
                StatusCode = 200,
                Success = true
            };
            return sr;

        }

        public async Task<StandardResult> CreateCourse(AdminCreateCourseDto dto)
        {
            var teacher = _repo.GetUserById(dto.TeacherId);
            if (teacher.Role == UserModel.UserRole.STUDENT || teacher.Role == UserModel.UserRole.ADMIN)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "معلمی با آیدی داده شده یافت نشد" },
                    StatusCode = 404,
                    Success = false,
                };
                return er;
            }
            var lesson = _lesson.GetLessonById(dto.LessonId);
            if (teacher is null)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "معلمی با آیدی داده شده یافت نشد" },
                    StatusCode = 404,
                    Success = false,
                };
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

            return sr;

        }

        public async Task<StandardResult> UpdateCourse(AdminUpdateCourseDto dto, int courseid)
        {
            var teacher = _repo.GetUserById(dto.TeacherId);
            if (teacher.Role == UserModel.UserRole.STUDENT || teacher.Role == UserModel.UserRole.ADMIN)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "معلمی با آیدی داده شده یافت نشد" },
                    StatusCode = 404,
                    Success = false,
                };
                return er;
            }
            var course = _course.GetCourseById(courseid);
            var lesson = _lesson.GetLessonById(dto.LessonId);

            if (teacher is null)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "معلمی با آیدی داده شده یافت نشد" },
                    StatusCode = 404,
                    Success = false,
                };
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
                return err;
            }


            course.CourseTitle = dto.CourseTitle;
            course.Cost = dto.Cost;
            course.TeacherId = teacher.Id;
            course.Teacher = teacher;
            course.LessonId = dto.LessonId;
            course.Lesson = lesson;
            course.StartDate = _shamsi.ToShamsi(dto.StartDate);
            course.EndDate = _shamsi.ToShamsi(dto.EndDate);
            course.Capacity = dto.Capacity;

            _course.Save();

            var tempDto = await _mapper.MapAsync(dto, new AdminShowUpdatedCourseDto());
            tempDto.StartDate = _shamsi.ToShamsi(dto.StartDate);
            tempDto.EndDate = _shamsi.ToShamsi(dto.EndDate);

            var sr = new StandardResult<AdminShowUpdatedCourseDto>
            {
                Messages = new List<string> { "Course Updated" },
                Result = tempDto,
                StatusCode = 201,
                Success = true
            };
            return sr;



        }
        
        public async Task<StandardResult> AddUserToCourse(int courseid, int userid)
        {
            var course = _course.GetCourseById(courseid);
            var user = _repo.GetUserById(userid);

            if (course is null)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "دوره ای با آیدی داده شده یافت نشد" },
                    StatusCode = 404,
                    Success = false
                };

                return er;
            }
            if (user is null)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "کاربری با آیدی داده شده یافت نشد" },
                    StatusCode = 404,
                    Success = false
                };

                return er;
            }

            if (user.Role == UserModel.UserRole.TEACHER)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "کاربر مورد نظر معلم است" },
                    StatusCode = 404,
                    Success = false
                };

                return er;
            }
            if (user.Role == UserModel.UserRole.ADMIN)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "کاربر مورد نظر ادمین است" },
                    StatusCode = 404,
                    Success = false
                };

                return er;
            }

            var check = _courseUser.CheckStudentForJoiningCourse(userid, courseid);
            if (check is null)
            {
                var newCourseUser = new CourseUserModel
                {
                    CourseId = courseid,
                    UserId = userid,
                    Course = course,
                    User = user
                };

                await _courseUser.Add(newCourseUser);

                var sr = new StandardResult
                {
                    Messages = new List<string>
                        { $"اضافه شد {course.CourseTitle} به دوره {user.LastName} {user.FirstName} کاربر " },
                    StatusCode = 201,
                    Success = true,
                };
                return sr;
            }

            var err = new StandardResult
            {
                Messages = new List<string> { "کاربر در این دوره است" },
                StatusCode = 404,
                Success = false,
            };
            return err;

        }

        public async Task<StandardResult> RemoveUserFromCourse(int courseid, int userid)
        {
            var course = _course.GetCourseById(courseid);
            var user = _repo.GetUserById(userid);

            if (course is null)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "دوره ای با آیدی داده شده یافت نشد" },
                    StatusCode = 404,
                    Success = false
                };

                return er;
            }
            if (user is null)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "کاربری با آیدی داده شده یافت نشد" },
                    StatusCode = 404,
                    Success = false
                };

                return er;
            }

            if (user.Role == UserModel.UserRole.TEACHER)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "کاربر مورد نظر معلم است" },
                    StatusCode = 404,
                    Success = false
                };

                return er;
            }
            if (user.Role == UserModel.UserRole.ADMIN)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "کاربر مورد نظر ادمین است" },
                    StatusCode = 404,
                    Success = false
                };

                return er;
            }

            var check = _courseUser.CheckStudentForJoiningCourse(userid, courseid);
            if (check is null)
            {
                var er = new StandardResult
                {
                    Messages = new List<string> { "دانشجو در این دوره ثبت نام نکرده است" },
                    StatusCode = 404,
                    Success = false,
                };
                return er;
            }

            await _courseUser.RemoveUserFromCourse(user.Id, course.Id);



            var err = new StandardResult
            {
                Messages = new List<string> { "کاربر از این دوره حذف شد" },
                StatusCode = 200,
                Success = true,
            };
            return err;

        }

        public async Task<StandardResult> GetAllSettings()
        {
            var setting = await _setting.GetAllSettings();

            var sr = new StandardResult<List<SettingModel>>
            {
                Messages = new List<string> { "Settings Retrived" },
                Result = setting,
                StatusCode = 200,
                Success = true,
            };
            return sr;
        }

        public async Task<StandardResult> UpdateSettings(UpdateSettingsDto dto)
        {
            var setting = await _setting.GetSettingById(1);


            setting.Theme = dto.Theme;
            setting.English = dto.English;
            setting.WebSiteIsActive = dto.WebSiteIsActive;

            await _context.SaveChangesAsync();

            var sr = new StandardResult<SettingModel>
            {
                Messages = new List<string> { "Settings Retrived" },
                Result = setting,
                StatusCode = 200,
                Success = true,
            };
            return sr;


        }
    }


}

