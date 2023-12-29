using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Data.Abtract;
using Service;
using Service.Abtract;
using Authentication.Services.Abtract;
using Authentication.Services;

namespace Infrastucture.Configuation
{
    public static class ConfigService
    {
        public static void RegisterDbContext(this IServiceCollection service, IConfiguration config)
        {
            service.AddDbContext<ShopiiContext>(option =>
            {
                option.UseSqlServer(config.GetConnectionString("SqlServer"));
            });
        }

        public static void RegisterIdentity(this IServiceCollection service)
        {
            service.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<ShopiiContext>()
            .AddDefaultTokenProviders();
        }

        public static void RegisterDI(this IServiceCollection service)
        {
            service.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            service.AddScoped<IDapperHelper, DapperHelper>();
            service.AddScoped<IUserServices, UserServices>();
            service.AddScoped<ITokenHandle, TokenHandle>();
            service.AddScoped<IUserTokenServices, UserTokenServices>();


            //Example
            service.AddScoped<IProductServices, ProductServices>();
        }
    }
}
