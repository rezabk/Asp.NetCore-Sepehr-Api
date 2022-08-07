using AspNetCoreRateLimit;
using BusinessLogic.BusinessLogics;
using BusinessLogic.BusinessLogics.Auth;
using BusinessLogic.BusinessLogics.Course;
using BusinessLogic.BusinessLogics.Lesson;
using BusinessLogic.BusinessLogics.User;
using BusinessLogic.Contracts;
using BusinessLogic.Repository;
using BusinessLogic.Utils;
using Filters;

namespace FinalPorject.Extensions
{
    public static class AddToolsServiceExtension
    {
        public static void AddTools(this IServiceCollection services)
        {
            services.AddScoped<GenerateToken>();
            services.AddTransient<IAdminRepository, AdminRepository>();
            services.AddTransient<IAuthRepository, AuthRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IBecomeMasterRepository, BecomeMasterRepository>();
            services.AddTransient<ILessonRepository, LessonRepository>();
            services.AddTransient<ICourseRepository, CourseRepository>();
            services.AddTransient<ICourseUserRepository, CourseUserRepository>();
            services.AddTransient<ISettingRepository, SettingRepository>();
            services.AddTransient<IAdminBl, AdminBl>();
            services.AddTransient<IAuthBl, AuthBl>();
            services.AddTransient<IUserBl, UserBl>();
            services.AddTransient<ILessonBl, LessonBl>();
            services.AddTransient<ICourseBl, CourseBl>();
            services.AddTransient<ICourseBl, CourseBl>();
            services.AddScoped<Mapper>();
            services.AddScoped<ShamasiCalendar>();
            services.AddScoped<HashPassword>();
            services.AddScoped<CreatePairToken>();
            services.AddScoped<GenerateToken>();
            services.AddScoped<StandardResultFilter>();
            services.AddCorsConfig();

            services.AddOptions();
            services.AddMemoryCache();
            services.AddInMemoryRateLimiting();

            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();


            services.AddControllersWithViews()
                 .AddNewtonsoftJson(options =>
                     options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                 );

        }
    }
}
