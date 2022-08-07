using DAL.DTO.User;
using DAL.Model;
using DAL.Model.CourseModels;
using Data;

namespace BusinessLogic.BusinessLogics.User
{
    public interface IUserBl
    {
        Task<StandardResult> GetProfile(int id);

        Task<StandardResult> GetCourses(int id);

        Task<StandardResult> JoinCourse(int id,int userid);

        Task<StandardResult> UpdateUser(UpdateUserDto dto, int id); 
        
        Task<StandardResult> BecomeMaster(BecomeMasterDto dto, int id);
    }
}
