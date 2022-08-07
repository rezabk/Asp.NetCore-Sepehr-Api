using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Model;

namespace BusinessLogic.Contracts
{
    public interface ISettingRepository
    {
        Task<List<SettingModel>> GetAllSettings();

        Task<SettingModel> GetSettingById(int id);
    }
}
