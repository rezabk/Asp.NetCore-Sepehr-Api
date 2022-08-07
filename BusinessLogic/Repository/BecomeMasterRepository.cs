using BusinessLogic.Contracts;
using DAL;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Repository
{
    public class BecomeMasterRepository : IBecomeMasterRepository
    {
        private readonly AcademyDbContext _context;

        public BecomeMasterRepository(AcademyDbContext context)
        {
            _context = context;
        }

        public async Task<List<BecomeMasterModel>> GetAllRequests()
        {
            return await _context.BecomeMaster.Where(x => x.Approvement == false).ToListAsync();
        }
        public BecomeMasterModel GetRequestById(int id)
        {
            return _context.BecomeMaster.SingleOrDefault(x => x.Id == id);
        }

        public BecomeMasterModel CheckUserRequest(int id)
        {
            return _context.BecomeMaster.SingleOrDefault(x => x.UserId == id);
        }


        public async Task<BecomeMasterModel> AddRequest(BecomeMasterModel becomeMasterModel)
        {
            await _context.BecomeMaster.AddAsync(becomeMasterModel);
            await _context.SaveChangesAsync();
            return becomeMasterModel;
        }

        public void Save()
        {
            _context.SaveChanges();
        }


    }
}
