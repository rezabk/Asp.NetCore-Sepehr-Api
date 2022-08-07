using DAL.Model;
using DAL.Model.CourseModels;

namespace BusinessLogic.Contracts
{
    public interface IUserRepository
    {
        UserModel GetProfile(int id);

        Task<UserModel> GetUserCourses(int id);

        UserModel CheckPhoneNumber(string phoneNumber,int userId);
        
        UserModel GetUserById(int id);

        void Save();
    }
}
