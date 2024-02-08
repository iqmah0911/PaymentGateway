using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsGLAccount
    {
        [Key]
        public int GLAccountID { get; set; }
        public string GLAccountCode { get; set; }
        
        [ForeignKey("BankID")]
        public SysBank Bank { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        
        [ForeignKey("CompanyID")]
        public SysCompany Company { get; set; }
        public bool IsActive { get; set; }
        public bool IsInUse { get; set; }
        public string BankAccount { get; set; }
    }
}
