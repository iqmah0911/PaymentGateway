using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class ApplicationUser: IdentityUser
    {

        [ForeignKey("UserID")]
        public int UserID { get; set; }

        public string Token { get; set; }
        public DateTime TokenExpiration { get; set; }
    }
}
