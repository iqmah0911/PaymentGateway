using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class SysServiceNotification
    {
        [Key]
        public int ServiceID { get; set; }

        public string Description { get; set; }

        public bool Status { get; set; }

        [ForeignKey("UserID")] 
        public SysUsers CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        
        public int ModifiedBy { get; set; }
           
        public DateTime DateModified { get; set; }



    }
}
