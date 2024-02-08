using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
//using PaymentGateway.Models;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Microsoft.Extensions.FileProviders;
using PaymentGateway21052021.Repositories.SysSetup;
using PaymentGateway21052021.Repositories.SysSetup.Interfaces;
using PaymentGateway21052021.Repositories;
//using PaymentGateway21052021.Installers;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Helpers.Interface;
using Microsoft.AspNetCore.Identity.UI.Services;
using PaymentGateway21052021.Installers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.DataProtection;
using System.IO;

namespace PaymentGateway21052021
{
    public class Startup
    {
       // public IHostingEnvironment Environment { get; }
        public Startup(IConfiguration configuration)  //, IHostingEnvironment environment)
        {
            Configuration = configuration;
            StaticConfig = configuration;
           /// Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public static IConfiguration StaticConfig { get; private set; }
        // This method gets called by the runtime. Use this method to add services to the container.
         
        public void ConfigureServices(IServiceCollection services)
        {
            services.InstallServicesFromAssembly(Configuration);

            #region "Initial settings used and working before the abstraction settings in Extensions & Installer folders was initiated"

            //Added service components used to resolve login issue 26-05-2021

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IMimeSender, MimeSender>();
            services.AddTransient<ISendGridmail, SendGridmail>();
            services.AddHttpContextAccessor();
            //services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton(typeof(Microsoft.Extensions.Logging.ILogger), typeof(Logger<Startup>));

            //services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromMinutes(1);
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.IsEssential = true; 
            //});

            //services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(15);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;

            });

            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                options.LoginPath = "/Identity/Account/Login";
                options.SlidingExpiration = true;
            });


            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.Expiration = TimeSpan.FromMinutes(10);
            //    options.LoginPath = "/Identity/Account/Login";
            //    options.LogoutPath = "/Identity/Account/Logout";
            //    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            //    options.SlidingExpiration = true;
            //});

            #endregion
        }

        //This method is used for creating User roles in Identity
        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { "Super Admin", "System Admin", "Agent", "Aggregator", "Merchant" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                //creating the roles and seeding them to the database
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new ApplicationRole(roleName));
                }
            }

        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider services)
        {
            //loggerFactory.AddFile("App_Log/Error_Log.txt");
            // loggerFactory.AddFile("Logs/ErrorTrace-{Date}.txt");
            var path = Directory.GetCurrentDirectory();
            loggerFactory.AddFile($"{path}\\Logs\\Log.txt");

            if (env.IsDevelopment()) //|| env.IsProduction())
            {
               app.UseDeveloperExceptionPage();
               // app.UseExceptionHandler("/Home/Error");
                app.UseDatabaseErrorPage();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseSession();
             
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default1",
                    template: "{area:exists}/{controller=HomeStore}/{action=Index}/{id?}/");

                routes.MapRoute(
                  name: "default",
                  template: "{controller=HomeStore}/{action=Welcome}/{id?}");

                routes.MapRoute(
                    name: "default1New",
                    template: "{area:exists}/{controller=HomeStore}/{action=Index}/{referenceNo?}/");

            });
        }
    }
}
