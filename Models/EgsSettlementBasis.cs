using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsSettlementBasis
    {
        [Key]
        public int SettlementBasisID { get; set; }
        public int ProductID { get; set; }
        public int ProductItemID { get; set; }
        public int SplittingRate { get; set; }
        public int SettlementTypeID { get; set; }
        public int SettlementIntervalID { get; set; }
        public int MerchantID { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
    }
}
