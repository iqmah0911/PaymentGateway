using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsAccountType
    {
        [Key]
        public int AccountTypeID { get; set; }
        public string AccountTypeName { get; set; }

        public DateTime DateCreated { get; set; }

        public SysUsers CreatedBy { get; set; }


        [InverseProperty("AccountType")]
        public IEnumerable<SysUsers> Users { get; set; }       
    }
}
