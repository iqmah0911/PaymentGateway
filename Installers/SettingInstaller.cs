using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway21052021.Areas.Identity.MenuConfiguration;
using PaymentGateway21052021.Installers.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Installers
{
    public class SettingInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<Config>(options =>
            {
                options.Menu = Configuration.GetSection("Menu").Get<List<Menu>>();
                
            });

            services.Configure<ConfigAdministrator>(options =>
            {
                options.MenuAdm = Configuration.GetSection("MenuAdmin").Get<List<MenuAdm>>();

            });

        }
    }
}
