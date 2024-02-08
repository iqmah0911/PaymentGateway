using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsMerchant
    {
        [Key]
        public int MerchantID { get; set; }
   
        public int MerchantType { get; set; }
        public string MerchantCode { get; set; }

        public string AccountNo { get; set; }
        public string Address { get; set; }
        public string KYC { get; set; }

        public string PhoneNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }

        [ForeignKey("BankID")]
        public SysBank Bank { get; set; }

        [InverseProperty("Merchant")]
        public IEnumerable<SysUsers> Users { get; set; }

        [InverseProperty("Merchant")]
        public IEnumerable<EgsProduct> Products { get; set; }

        [InverseProperty("Merchant")]
        public IEnumerable<EgsSettlementLog> SettlementLogs { get; set; }
        [InverseProperty("Merchant")]
        public IEnumerable<EgsSettlementSummary> SettlementSummarys { get; set; }



        //[InverseProperty("Merchant")]
        //public IEnumerable<SysMerchantMapping> MerchantMappings { get; set; }
    }
}
