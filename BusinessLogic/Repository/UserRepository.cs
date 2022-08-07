using BusinessLogic.Contracts;
using DAL;
using DAL.Model;
using DAL.Model.CourseModels;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AcademyDbContext _context;

        public UserRepository(AcademyDbContext context)
        {
            _context = context;
        }
        public UserModel GetProfile(int id)
        {
            return _context.Users.SingleOrDefault(x => x.Id == id);
        }


        public async Task<UserModel> GetUserCourses(int id)
        {
            return (await _context.Users.Include((x => x.Courses)).SingleOrDefaultAsync(x => x.Id == id));
        }

        public UserModel CheckPhoneNumber(string phoneNumber, int userId)
        {
            return _context.Users.SingleOrDefault(x => x.PhoneNumber == phoneNumber && x.Id != userId);
        }

        public UserModel GetUserById(int id)
        {
            return _context.Users.SingleOrDefault(x => x.Id == id && x.IsDeleted == false);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
