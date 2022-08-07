using DAL.DTO;
using DAL.DTO.Admin;
using Data;

namespace BusinessLogic.BusinessLogics
{
    public interface IAdminBl
    {

        Task<StandardResult> GetAllUsers();

        Task<StandardResult> GetUserById(int id);

        Task<StandardResult> Availablity(int id);

        Task<StandardResult> GetAllBecomeMasterRequests();

        Task<StandardResult> GetBecomeMasterRequestById(int id);

        Task<StandardResult> ApproveMasterById(int id);

        Task<StandardResult> CheckCreateUser(AdminCreateUserDto dto);

        Task<StandardResult> CheckUpdateUser(AdminUpdateUserDto dto, int id);

        Task<StandardResult> CheckRemoveUser(int id);

        Task<StandardResult> CreateCourse(AdminCreateCourseDto dto); 
        Task<StandardResult> UpdateCourse(AdminUpdateCourseDto dto,int courseid);

        Task<StandardResult> AddUserToCourse(int courseid, int userid);
        Task<StandardResult> RemoveUserFromCourse(int courseid, int userid);

        Task<StandardResult> GetAllSettings(); 
        
        Task<StandardResult> UpdateSettings(UpdateSettingsDto dto);
    }
}
