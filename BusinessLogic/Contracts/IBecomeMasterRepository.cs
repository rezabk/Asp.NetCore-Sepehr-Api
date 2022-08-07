using DAL.Model;

namespace BusinessLogic.Contracts
{
    public interface IBecomeMasterRepository
    {
        Task<List<BecomeMasterModel>> GetAllRequests();
        Task<BecomeMasterModel> AddRequest(BecomeMasterModel becomeMasterModel);  
        BecomeMasterModel GetRequestById(int id);

        BecomeMasterModel CheckUserRequest(int id);
        void Save();

    }
}
