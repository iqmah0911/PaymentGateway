using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Helpers.Interface
{
    public interface IMimeSender
    {
        Task Execute(string apiKey, string subject, string msg, string email, string attachment = null, string CopyMail = null);


    }
}
