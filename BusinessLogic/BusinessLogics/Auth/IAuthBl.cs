using DAL.DTO.Auth;
using Data;

namespace BusinessLogic.BusinessLogics.Auth
{
    public interface IAuthBl
    {
        Task<StandardResult> Register(RegisterDto dto);  
        Task<StandardResult> Login(LoginDto dto);
    }
}
