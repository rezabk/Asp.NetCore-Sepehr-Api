using BusinessLogic.Contracts;
using DAL;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Repository
{
    public class LessonRepository : ILessonRepository
    {
        private readonly AcademyDbContext _context;

        public LessonRepository(AcademyDbContext context)
        {
            _context = context;
        }


        public async Task<List<LessonModel>> GetAllLessons()
        {
            return await _context.Lessons.Where(x => x.IsDeleted == false).ToListAsync();
        }

        public LessonModel GetLessonById(int id)
        {
            return _context.Lessons.SingleOrDefault(x => x.Id == id && x.IsDeleted == false);
        }

        public async Task<LessonModel> AddLesson(LessonModel lessonModel)
        {
            await _context.Lessons.AddAsync(lessonModel);
            await _context.SaveChangesAsync();
            return lessonModel;
        }

        public async Task<bool> CheckLessonName(string lessonName)
        {
            return await _context.Lessons.AnyAsync(x => x.LessonName == lessonName);
        }

        public async Task<bool> RemoveLesson(int lessonId)
        {
            var res = GetLessonById(lessonId);
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
