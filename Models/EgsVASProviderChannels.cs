using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsVASProviderChannels
    {

        [Key]
        public int ProviderChannelID { get; set; }

        public string ServiceProvider { get; set; }

        public string ServiceCategory { get; set; }

        public string ServiceDetails { get; set; }

        public int Priority { get; set; }

        public DateTime DateCreated { get; set; }

        public int Createdby { get; set; }

        public int Modifiedby { get; set; }

        public DateTime DateModified { get; set; }

        public double Commission { get; set; }

        public double Cap { get; set; }

        public string ApiKey { get; set; }
        public string Logo { get; set; }
        public string Notification { get; set; }

        public string Verification { get; set; }




    }
}
