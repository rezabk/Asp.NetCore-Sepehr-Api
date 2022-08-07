using DAL.Model;

namespace BusinessLogic.Contracts
{
    public interface IAdminRepository
    {
        Task<List<UserModel>> GetAllUsers();
        UserModel GetUserById(int id);


        Task<UserModel> AddUser(UserModel userModel);

        Task<bool> RemoveUser(int id);


        void Save();


    }
}
