using Application.Services.Car;
using Application.Services.Photo;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<IPhotoService, PhotoService>();

            return services;
        }
    }
}
