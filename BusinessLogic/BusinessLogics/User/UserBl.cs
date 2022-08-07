using BusinessLogic.Contracts;
using BusinessLogic.Utils;
using DAL.DTO;
using DAL.DTO.User;
using DAL.Model;
using Data;
using DAL.Model.CourseModels;

namespace BusinessLogic.BusinessLogics.User
{
    public class UserBl : IUserBl
    {
      private readonly IUserRepository _user;
        private readonly IBecomeMasterRepository _master;
        private readonly Mapper _mapper;
        private readonly HashPassword _hash;
        private readonly ShamasiCalendar _shamasi;
        private readonly ICourseUserRepository _courseUser;
        private readonly ICourseRepository _course;

        public UserBl(IUserRepository user, IBecomeMasterRepository master, Mapper mapper, HashPassword hash, ShamasiCalendar shamasi, ICourseUserRepository courseUser, ICourseRepository course)
        {
            _user = user;
            _master = master;
            _mapper = mapper;
            _hash = hash;
            _shamasi = shamasi;
            _courseUser = courseUser;
            _course = course;
        }
        public async Task<StandardResult> GetProfile(int id)
        {
            var user = _user.GetUserById(id);
            if (user is null)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "کاربری با آیدی وارد شده پیدا نشد" },
                    StatusCode = 404,
                    Success = false,
                };
                return err;
            }

            var showUser = await _mapper.MapAsync(user, new ShowUserDto());
            var sr = new StandardResult<ShowUserDto>
            {
                Messages = new List<string> { "User Retrived" },
                Result = showUser,
                StatusCode = 200,
                Success = true,
            };
            return sr;

        }

        public async Task<StandardResult> GetCourses(int id)
        {
            var user = _user.GetUserById(id);
            var courses = await _courseUser.GetUserCourses(id);

            if (user.Role == UserModel.UserRole.STUDENT && courses.Count == 0)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "شما در هیچ دوره ای شرکت نکردید" },
                    StatusCode = 400,
                    Success = false
                };
                return err;

            }

            if (user.Role == UserModel.UserRole.TEACHER && courses.Count == 0)
            {
                if (courses.Count == 0)
                {
                    var err = new StandardResult
                    {
                        Messages = new List<string> { "شما هیج دوره ای تدریس نکرده اید" },
                        StatusCode = 400,
                        Success = false
                    };
                    return err;
                }
            }

            var userCourses = new List<ShowUserCoursesDto>();
            foreach (var item in courses)
            {
                var course = _course.GetCourseById(item.CourseId);
                var tempDto = await _mapper.MapAsync(item.Course, new ShowUserCoursesDto());
                userCourses.Add(tempDto);

            }

            var sr = new StandardResult<List<ShowUserCoursesDto>>
            {
                Messages = new List<string> { "ok" },
                Result = userCourses,
                StatusCode = 200,
                Success = true,
            };
            return sr;
        }


        public async Task<StandardResult> JoinCourse(int id, int userid)
        {
            var user = _user.GetUserById(userid);
            var course = _course.GetCourseById(id);

            if (user.Role == UserModel.UserRole.TEACHER)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "شما معلم هستید و نمی توانید در دوره ای شرکت کنید" },
                    StatusCode = 404,
                    Success = false
                };
                return err;
            }
            if (course is null)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "دوره ای با آیدی داده شده وجود ندارد" },
                    StatusCode = 404,
                    Success = false
                };
                return err;
            }

            var joinCourse = new CourseUserModel
            {
                CourseId = course.Id,
                UserId = user.Id,
                Course = course,
                User = user,
            };
            var check = _courseUser.CheckStudentForJoiningCourse(userid, id);
            if (check is null)
            {
                await _courseUser.Add(joinCourse);
                user.Courses.Add(course);

                course.Students.Add(user);
                _course.Save();

                var sr = new StandardResult
                {
                    Messages = new List<string> { "درخواست شما با موفقیت ثبت شد" },
                    StatusCode = 200,
                    Success = true
                };
                return sr;
            }

            var er = new StandardResult
            {
                Messages = new List<string> { "شما قبلا در این دوره شرکت کرده اید" },
                StatusCode = 404,
                Success = false
            };
            return er;
        }


        public async Task<StandardResult> UpdateUser(UpdateUserDto dto, int id)
        {
            var user = _user.GetUserById(id);
            if (user is null)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "کاربری با آیدی وارد شده پیدا نشد" },
                    StatusCode = 404,
                    Success = false,
                };
                return err;
            }

            if (string.IsNullOrWhiteSpace(dto.FirstName))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "لطفا نام خود را وارد کنید" },
                    StatusCode = 404,
                    Success = false
                };
                return err;
            }
            if (string.IsNullOrWhiteSpace(dto.LastName))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "لطفا نام خوانوادگی خود را وارد کنید" },
                    StatusCode = 404,
                    Success = false
                };
                return err;
            }
            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "لطفا رمز عبور خود را وارد کنید" },
                    StatusCode = 404,
                    Success = false
                };
                return err;
            }
            if (string.IsNullOrWhiteSpace(dto.RePassword))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "لطفا تکرار رمز عبور خود را وارد کنید" },
                    StatusCode = 404,
                    Success = false
                };
                return err;
            }

            if (!dto.Password.Equals(dto.RePassword))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "رمز عبور با تکرار آن مطابقت ندارد" },
                    StatusCode = 404,
                    Success = false,
                };
                return err;
            }

            var checkPhoneNumber = _user.CheckPhoneNumber(dto.PhoneNumber,user.Id);
            if (checkPhoneNumber is null)
            {
                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.Password = _hash.GetHash(dto.Password);
                user.Address = dto.Address;
                user.PhoneNumber = dto.PhoneNumber;

                _user.Save();

                var showUpdatedUser = await _mapper.MapAsync(dto, new ShowUpdatedUserDto());
                var sr = new StandardResult<ShowUpdatedUserDto>
                {
                    Messages = new List<string> { "UserModel Updated" },
                    Result = showUpdatedUser,
                    StatusCode = 201,
                    Success = true
                };

                return sr;
            }

            var er = new StandardResult
            {
                Messages = new List<string> { "شماره موبایل وارد شده در سیستم وجود دارد" },
                StatusCode = 404,
                Success = false
            };

            return er;
        }

        public async Task<StandardResult> BecomeMaster(BecomeMasterDto dto, int id)
        {
            var user = _user.GetUserById(id);
            if (user is null)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "کاربری با آیدی وارد شده پیدا نشد" },
                    StatusCode = 404,
                    Success = false,
                };
                return err;
            }

            if (user.Role == UserModel.UserRole.ADMIN)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "شما ادمین هستید و نمیتوانید درخواست تغییر وضعیت به معلم بدهید" },
                    StatusCode = 404,
                    Success = false,
                };
                return err;
            }
            if (user.Role == UserModel.UserRole.TEACHER)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "شما معلم هستید و نمیتوانید درخواست تغییر وضعیت به معلم بدهید" },
                    StatusCode = 404,
                    Success = false,
                };
                return err;
            }
            var request = new BecomeMasterModel
            {
                Description = dto.Description,
                RequestDate = _shamasi.ToShamsi(DateTime.Now),
                UserId = user.Id,
                UserEmail = user.Email,
                Approvement = false,
            };


            var checkUserRequest = _master.CheckUserRequest(id);
            if (checkUserRequest is null)
            {
                await _master.AddRequest(request);

                var sr = new StandardResult
                {
                    Messages = new List<string> { "درخواست تغییر وضعیت به معلم ثبت شد" },
                    StatusCode = 201,
                    Success = true
                };
                return sr;
            }
            var er = new StandardResult
            {
                Messages = new List<string> { "شما قبلا درخواست داده اید" },
                StatusCode = 409,
                Success = false
            };
            return er;



        }
    }
}
