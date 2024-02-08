using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;

[assembly: HostingStartup(typeof(PaymentGateway21052021.Areas.Identity.IdentityHostingStartup))]
namespace PaymentGateway21052021.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
         public void Configure(IWebHostBuilder builder)
            {
        
               builder.ConfigureServices((context, services) => {
               });
            }
        

        //public void Configure(IWebHostBuilder builder)
        //{
        //    builder.ConfigureServices((context, services) => {
        //        services.AddDbContext<ApplicationDBContext>(options =>
        //            options.UseSqlServer(
        //                context.Configuration.GetConnectionString("DefaultConnection")));

        //        //services.AddDefaultIdentity<IdentityUser>()
        //        //services.AddIdentity<ApplicationUser, ApplicationRole>() 
        //        //    .AddEntityFrameworkStores<ApplicationDBContext>()
        //        //   .AddSignInManager<SignInManager<ApplicationUser>>()
        //        //   .AddDefaultTokenProviders();

        //    });
        //}
    }
}