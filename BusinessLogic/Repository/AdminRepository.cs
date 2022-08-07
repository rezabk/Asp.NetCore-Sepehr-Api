using BusinessLogic.Contracts;
using DAL;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AcademyDbContext _context;

        public AdminRepository(AcademyDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            return await _context.Users.Where(x => x.IsDeleted == false).ToListAsync();
        }

        public UserModel GetUserById(int id)
        {
            return _context.Users.SingleOrDefault(x => x.Id == id && x.IsDeleted == false);
        }


        public async Task<UserModel> AddUser(UserModel userModel)
        {
            await _context.Users.AddAsync(userModel);
            await _context.SaveChangesAsync();
            return userModel;
        }

        public async Task<bool> RemoveUser(int id)
        {
            var res = GetUserById(id);
            res.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }


        public void Save()
        {
            _context.SaveChanges();
        }


    }
}

