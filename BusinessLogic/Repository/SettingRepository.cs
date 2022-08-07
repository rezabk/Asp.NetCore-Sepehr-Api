using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Contracts;
using DAL;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Repository
{
    public class SettingRepository : ISettingRepository
    {
        private readonly AcademyDbContext _context;

        public SettingRepository(AcademyDbContext context)
        {
            _context = context;
        }

        public async Task<List<SettingModel>> GetAllSettings()
        {
            return await _context.Settings.ToListAsync();
        }

        public async Task<SettingModel> GetSettingById(int id)
        {
            return await _context.Settings.SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
