using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsUpgradeAccount
    {
        [Key]
       // [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UpgradeAccountID { get; set; }
        
        [ForeignKey("UserID")]
        public SysUsers User { get; set; }

        // [ForeignKey("RoleRequestID")]
        //public SysRole RoleRequest { get; set; } 

        public int RoleRequestID { get; set; }
         
        public int UserTypeRequestID { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsProcessed { get; set; }
        public int ProcessedBy { get; set; }
        public DateTime DateProcessed { get; set; }
        public int RejectedBy { get; set; }
        public DateTime DateRejected { get; set; }
        public string Comment { get; set; }

    }
}
