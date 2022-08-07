using System.Security.Cryptography.X509Certificates;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Mapping
{
    public class CourseUserMapping : IEntityTypeConfiguration<CourseUserModel>
    {
        public void Configure(EntityTypeBuilder<CourseUserModel> builder)
        {
            builder.ToTable("CourseUsers");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Course).WithMany(x => x.CourseUser).HasForeignKey(x => x.CourseId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User).WithMany(x => x.CourseUser).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);

        }
    }
}
