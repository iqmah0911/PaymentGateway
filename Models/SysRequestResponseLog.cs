using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class SysRequestResponseLog
    {
        [Key]
        public int RequestResponseID { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public DateTime DateCreated { get; set; }
        public string Message { get; set; }
        public string StatusCode { get; set; }
        public string Provider { get; set; }
        public string TransactionType { get; set; }

    }
}
