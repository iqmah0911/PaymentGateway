using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsAccountLookup
    {
        [Key]
        public int lookupID { get; set; }

        public string AccountName { get; set; }

        public string BVN { get; set; }

        public string ClientId { get; set; }

        public string AccountId { get; set; }

        public string AccountNumber { get; set; }

        public string BankName { get; set; }

        public string BankCode { get; set; }


    }
}
