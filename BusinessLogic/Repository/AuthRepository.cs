using BusinessLogic.Contracts;
using DAL;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AcademyDbContext _context;

        public AuthRepository(AcademyDbContext context)
        {
            _context = context;
        }

        public async Task<UserModel> RegisterUser(UserModel userModel)
        {
            await _context.Users.AddAsync(userModel);
            await _context.SaveChangesAsync();
            return userModel;
        }

        public UserModel GetUserByEmailAndPassword(string email, string password)
        {
            return _context.Users.SingleOrDefault(x => x.Email == email && x.Password == password && x.IsActive == true);
        }

        public async Task<bool> CheckEmailForRegister(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<bool> CheckPhoneNumberForRegister(string phoneNumber)
        {
            return await _context.Users.AnyAsync(x => x.PhoneNumber == phoneNumber);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
