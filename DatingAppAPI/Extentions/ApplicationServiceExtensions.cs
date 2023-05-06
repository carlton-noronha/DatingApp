using DatingAppAPI.Data;
using DatingAppAPI.Helpers;
using DatingAppAPI.Repositories;
using DatingAppAPI.Repositories.Implementations;
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
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(config => {
                config.AddProfile(new AutoMapperProfiles());
            });
            return services;
        }
    }
}