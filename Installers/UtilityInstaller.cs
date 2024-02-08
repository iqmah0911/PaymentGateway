using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway21052021.Installers.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Installers
{
    public class UtilityInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            //var mappingConfig = new MapperConfiguration(mc =>
            //{
            //    mc.AddProfile(new AutoMapperProfile());
            //});

            //IMapper mapper = mappingConfig.CreateMapper();
            //services.AddSingleton(mapper);

            //for email client injection
            //services.AddScoped<IEmailSender, EmailSender>();
            //services.AddScoped<EmailSender>();
            //services.AddScoped<SMSService>();
            
        }
    }
}
