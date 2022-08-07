using DAL.Model;

namespace BusinessLogic.Contracts
{
    public interface IAuthRepository
    {
        Task<UserModel> RegisterUser(UserModel userModel);

        UserModel GetUserByEmailAndPassword(string email, string password);

        Task<bool> CheckEmailForRegister(string email);  
        
        Task<bool> CheckPhoneNumberForRegister(string phoneNumber);
        
        void Save();
    }
}
