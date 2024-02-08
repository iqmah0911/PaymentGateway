using Microsoft.AspNetCore.Mvc.Rendering;
using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{

    public class SettlementBasisViewModel
    {
        public int SubMerchantID { get; set; }
        public int SplittingRate { get; set; }
        public int ProductItemID { get; set; }
        public int ProductID { get; set; }
        public int SettlementIntervalID { get; set; }
        public int SettlementTypeID { get; set; }
    }

    public class EgsSettlementBasisViewModel  
    {
      
        public int SettlementBasisID { get; set; }

        public int ProductItemID { get; set; }
        public int ProductID { get; set; }

        public int SettlementTypeID { get; set; }
        public int SettlementIntervalID { get; set; }
        public int MerchantID { get; set; }
        public int SubMerchantID { get; set; }
        public int SplittingRate { get; set; }
        public string SubMerchantName { get; set; }
        public string MerchantName { get; set; }

        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }

        public List<EgsSettlementLogs> HoldSettlementLog { get; set; }

        public IEnumerable<HoldDisplaySettlementBasisViewModel> HoldSettlementBasis { get; set; }

        //public DisplaySettlementBasisViewModel displaySettlementBasis { get; set; }

    }

    public class EgsSettlementLogs
    { 
        public int SettlementLogID { get; set; }
        public int ProductItemID { get; set; }
        public double Totalcollection { get; set; }
        public int SettlementIntervalID { get; set; }
        public int MerchantID { get; set; }
        public double MerchantAmount { get; set; }
        public int SettlementTypeID { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public bool IsPaid { get; set; }



    }




    public class DisplaySettlementBasisViewModel : EgsSettlementBasisViewModel
    {
        public Pager Pager { get; set; }
        public IEnumerable<EgsSettlementBasisViewModel> Items { get; set; }
    }

    public class HoldDisplaySettlementBasisViewModel : EgsSettlementBasisViewModel
    {
        //public IEnumerable<object> Items { get; set; }

        public IEnumerable<DisplaySettlementBasisViewModel> HoldAllSettlementBasis { get; set; }
        public List<DisplaySettlementBasisViewModel> HoldAllSettlementBasisList { get; set; }
        public Pager Pager { get; set; }
    }



}
