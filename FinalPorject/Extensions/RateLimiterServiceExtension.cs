using AspNetCoreRateLimit;

namespace FinalPorject.Extensions
{
    public class RateLimiterServiceExtension
    {
        public void ConfigureServices(IServiceCollection services)
        {
        
            services.AddOptions();

         
            services.AddMemoryCache();

     
            services.AddInMemoryRateLimiting();
   
            services.AddMvc();

           
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }

    }
}
