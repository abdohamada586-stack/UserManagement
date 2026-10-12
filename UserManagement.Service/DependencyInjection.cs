using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Service.Interfaces;
using UserManagement.Service.Services;

namespace UserManagement.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddService(
            this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtService, JwtService>();

            return services;
        }
    }
}