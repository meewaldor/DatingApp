using DatingAPI.Data;
using DatingAPI.Helpers;
using DatingAPI.Interfaces;
using DatingAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace DatingAPI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices (this IServiceCollection services, IConfiguration config)
        {
            // Connect to DB
            services.AddDbContext<DataContext>(option =>
            {
                option.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            //
            services.AddCors();

            // how long the service is available
            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILikeRepository, LikesRepository>();

            // configure auto mapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // configure Cloudinary setting
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

            // configure cloud uploading photo service
            services.AddScoped<IPhotoService, PhotoService>();

            services.AddScoped<LogUserActivity>();
            return services;
        }
    }
}
