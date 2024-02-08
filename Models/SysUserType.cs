using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class SysUserType
    {
        [Key]
        public int UserTypeID { get; set; }

        public string UserType { get; set; }
         
        public DateTime DateCreated { get; set; }

        public int CreatedBy { get; set; }

        //Added on 6-9-2023

        public double DailyLimit { get; set; }

        public double TransactionLimit { get; set; }



        [InverseProperty("UserType")]
        public IEnumerable<SysUsers> Users { get; set; }

         


    }
}
