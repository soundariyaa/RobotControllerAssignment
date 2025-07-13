using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RobotControllerApi.Core.Repositories;
using RobotControllerApi.Core.Services.Interfaces;
using RobotControllerApi.Infrastructure.Repositories;
using RobotControllerApi.Infrastructure.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControllerApi.Infrastructure.Extensions
{
   public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<RoboDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            return services
                .AddScoped<IRobotService, RobotService>()
                .AddScoped<IRobotRepository, RobotRepository>()
                .AddDbContext<RoboDbContext>(); 
        }
    }
}
