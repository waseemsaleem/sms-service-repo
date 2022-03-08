using ApiService.Services;
using ApiService.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiUnitTest
{
    public class Startup
    {

        
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<accountdbContext>(options =>
            //    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IPhoneService, PhoneService>();
        }
    }
}