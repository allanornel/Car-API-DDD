﻿using Domain.Interfaces;
using Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<IDbConnectionFactory>(_ => new DbConnectionFactory(connectionString));
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();

            return services;
        }
    }
}
