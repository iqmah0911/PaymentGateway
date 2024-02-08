using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway21052021.Models
{
    public class EgsSettlementBasisRe
    {
        [Key]
        public int SettlementBasisID { get; set; }

        [Display(Name = "Settlement Basis")]
        public string SettlementBasis { get; set; }

        //[ForeignKey("ProductID")]
        //public EgsProduct ProductID { get; set; }


        //[ForeignKey("SettlementModeID")]
        //public EgsSettlementMode SettlementModeID { get; set; }


        //[ForeignKey("SettlementRateTypeID")]
        //public EgsSettlementRateType SettlementRateTypeID { get; set; }

        public double SettlementRate1 { get; set; }
        public double SettlementRate2 { get; set; }
        public double SettlementRate3 { get; set; }
        public double SettlementRate4 { get; set; }
        public double SettlementRate5 { get; set; }
        public double SettlementRate { get; set; }

        //[ForeignKey("UserTypeID")]
        //public SysUserType UserTypeID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
