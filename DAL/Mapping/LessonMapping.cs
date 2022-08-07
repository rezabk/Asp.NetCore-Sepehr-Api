using DAL.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Mapping
{
    public class LessonMapping : IEntityTypeConfiguration<LessonModel>
    {
        public void Configure(EntityTypeBuilder<LessonModel> builder)
        {
            builder.HasMany(x => x.Courses).WithOne(x => x.Lesson).HasForeignKey(x => x.LessonId);
        }
    }
}
