using BusinessLogic.Contracts;
using BusinessLogic.Utils;
using DAL;
using DAL.DTO.Auth;
using DAL.Model;
using Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BusinessLogic.BusinessLogics.Auth
{
    public class AuthBl : IAuthBl
    {

        private readonly HashPassword _hash;
        private readonly IAuthRepository _auth;
        private readonly CreatePairToken _token;
        private readonly Serilog.ILogger _logger = Log.Logger;
        public AuthBl(HashPassword hash, IAuthRepository auth, CreatePairToken token)
        {
            _hash = hash;
            _auth = auth;
            _token = token;
        }
        public async Task<StandardResult> Register(RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FirstName))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "لطفا نام خود را وارد کنید" },
                    StatusCode = 404,
                    Success = false
                };
                _logger.Error("FinalProject : /Register success:false");
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
                _logger.Error("FinalProject : /Register success:false");
                return err;
            }
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "لطفا ایمیل خود را وارد کنید" },
                    StatusCode = 404,
                    Success = false
                };
                _logger.Error("FinalProject : /Register success:false");
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
                _logger.Error("FinalProject : /Register success:false");
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
                _logger.Error("FinalProject : /Register success:false");
                return err;
            }
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "لطفا شماره موبایل خود را وارد کنید" },
                    StatusCode = 404,
                    Success = false
                };
                _logger.Error("FinalProject : /Register success:false");
                return err;
            }


            var checkEmailForRegister = await _auth.CheckEmailForRegister(dto.Email);
            if (checkEmailForRegister)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "این ایمیل قبلا در سایت ثبت نام کرده است" },
                    StatusCode = 404,
                    Success = false
                };
                _logger.Error("FinalProject : /Register success:false");
                return err;
            }
            var checkPhoneNumberForRegister = await _auth.CheckPhoneNumberForRegister(dto.PhoneNumber);
            if (checkPhoneNumberForRegister)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "این شماره موبایل در سایت ثبت نام کرده است" },
                    StatusCode = 404,
                    Success = false
                };
                _logger.Error("FinalProject : /Register success:false");
                return err;
            }

            if (dto.Password != dto.RePassword)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "پسوورد وارد شده با تکرار آن مطابقت ندارد" },
                    StatusCode = 404,
                    Success = false
                };
                _logger.Error("FinalProject : /Register success:false");
                return err;
            }

            var newUser = new UserModel
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Password = _hash.GetHash(dto.Password),
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Role = UserModel.UserRole.STUDENT,
                Address = dto.Address,
                IsActive = true,
                IsDeleted = false,
            };
            await _auth.RegisterUser(newUser);
            var (tokens, refreshEx) = _token.GeneratePairToken(newUser);
            newUser.Refresh(tokens.RefreshToken, DateTime.UtcNow.AddDays(refreshEx));
            _auth.Save();
            var sr = new StandardResult<PairTokenDto>
            {
                Messages = new List<string> { "Register Successful" },
                Result = tokens,
                StatusCode = 201,
                Success = true
            };
            _logger.Information("FinalProject : /Register success:true");
            return sr;
        }

        public async Task<StandardResult> Login(LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "لطفا ایمیل خود را وارد کنید" },
                    StatusCode = 404,
                    Success = false
                };
                _logger.Error("FinalProject : /Login success:false");
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
                _logger.Error("FinalProject : /Login success:false");
                return err;
            }

            var hashPassword = _hash.GetHash(dto.Password);

            var user = _auth.GetUserByEmailAndPassword(dto.Email, hashPassword);
            if (user is null)
            {
                var err = new StandardResult
                {
                    Messages = new List<string> { "ایمیل یا رمز عبور اشتباه است" },
                    StatusCode = 404,
                    Success = false
                };
                _logger.Error("FinalProject : /Login success:false");
                return err;
            }

            var (tokens, refreshEx) = _token.GeneratePairToken(user);
            user.Refresh(tokens.RefreshToken, DateTime.UtcNow.AddDays(refreshEx));
            _auth.Save();

            var sr = new StandardResult<PairTokenDto>
            {
                Messages = new List<string> { "ورود موفقیت آمیز بود" },
                Result = tokens,
                StatusCode = 202,
                Success = true,
            };
            _logger.Information("FinalProject : /Login success:true");
            return sr;
        }
    }
}
