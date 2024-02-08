using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsClient
    {
        [Key]
        public int ID { get; set; }

        public string CompanyName { get; set; }

        public string Email { get; set; }
         
        public string PhoneNumber { get; set; }

        public string WalletCredentials { get; set; }

        public string IPAddress { get; set; }

        public string ClientID { get; set; }
        public string ClientSecret { get; set; }

        public string Token { get; set; }
        public DateTime DateCreated { get; set; }

        public int CreatedBy { get; set; }

        public int WalletID { get; set; }
         
        public bool Expiration { get; set; }




    }
}
