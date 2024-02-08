using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Helpers.Interface
{
    public interface ISendGridmail
    {
        Task Execute(string email, string subject, string htmlMessage);
    }
}
