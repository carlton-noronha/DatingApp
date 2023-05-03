using DatingAppAPI.Data;
using DatingAppAPI.Services;
using DatingAppAPI.Services.Implementations;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Extentions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}