using Microsoft.AspNetCore.Mvc.Rendering;
using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{

    public class AggregatorCommissionViewModel
    {
        public int AgentID { get; set; }
        public int SplittingRate { get; set; }
        public int ProductItemID { get; set; }
        public int ProductID { get; set; }
        public int SettlementIntervalID { get; set; }
        public int SettlementTypeID { get; set; }
    }

    public class AggregatorCommModel : BasicObjectsViewModel
    {
        public int AggregatorCommissionID { get; set; }

        public int ProductItemID { get; set; }
        public int ProductID { get; set; }

        public int SettlementTypeID { get; set; }
        public int SettlementIntervalID { get; set; }
        public int AggregatorID { get; set; }
        public int SplittingRate { get; set; }
    }

    public class EgsAggregatorCommissionViewModel
    {

        public int AggregatorCommissionID { get; set; }

        public int ProductItemID { get; set; }
        public int ProductID { get; set; }

        public int SettlementTypeID { get; set; }
        public int SettlementIntervalID { get; set; }
        public int AggregatorID { get; set; }
        public int AgentID { get; set; }
        public int SplittingRate { get; set; }
        public string AgentName { get; set; }
        public string AggregatorName { get; set; }

        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }

        public List<EgsSettlementLogs> HoldSettlementLog { get; set; }

        public IEnumerable<HoldDisplayAggregatorCommissionViewModel> HoldSettlementBasis { get; set; }

    }

    public class AggregatorCommResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        //public DocumentsViewModel docuument { get; set; }
        public List<EgsAggregatorCommissionViewModel> aggregator { get; set; }
    }

    //public class EgsSettlementLogs
    //{
    //    public int SettlementLogID { get; set; }
    //    public int ProductItemID { get; set; }
    //    public double Totalcollection { get; set; }
    //    public int SettlementIntervalID { get; set; }
    //    public int MerchantID { get; set; }
    //    public double MerchantAmount { get; set; }
    //    public int SettlementTypeID { get; set; }
    //    public DateTime DateCreated { get; set; }
    //    public int CreatedBy { get; set; }
    //    public bool IsPaid { get; set; }



    //}




    public class DisplayAggregatorCommissionViewModel : EgsAggregatorCommissionViewModel
    {
        public Pager Pager { get; set; }
        public IEnumerable<EgsAggregatorCommissionViewModel> Items { get; set; }
    }

    public class HoldDisplayAggregatorCommissionViewModel : EgsAggregatorCommissionViewModel
    {
        //public IEnumerable<object> Items { get; set; }

        public IEnumerable<DisplaySettlementBasisViewModel> HoldAllSettlementBasis { get; set; }
        public List<DisplaySettlementBasisViewModel> HoldAllSettlementBasisList { get; set; }
        public Pager Pager { get; set; }
    }

}
