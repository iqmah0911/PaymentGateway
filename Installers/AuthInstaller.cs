using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Installers.Interface;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Installers
{
    public class AuthInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);
              
            services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
            {
                opt.Lockout.MaxFailedAccessAttempts = 3;
                //opt.Password.RequiredLength = 8;
                //opt.Password.RequireNonAlphanumeric = true;
                //opt.Password.RequireUppercase = true;
                //opt.Password.RequireLowercase = true;

            }).AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<UnitOfWork>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDistributedMemoryCache();

         
 
        }
    }
}
