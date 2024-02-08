using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.Invoices.Models
{
    public class InvoicesViewModel
    {
        public string ReferenceNo { get; set; }
        public string ServiceNumber { get; set; }
        public double Amount { get; set; }

        public DateTime DateCreated { get; set; }
    }


    public class EInvoicesViewModel
    {
        public string CustReference { get; set; }
        public string ServiceNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public double TotalAmount { get; set; }
        public List<EgsProductItem> ItemList { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public string ThirdPartyCode { get; set; }
        public string AgentName { get; set; }
        public string BranchName { get; set; }
        public string RegistrationNo { get; set; }

    }


    public class ERegInvoicesViewModel
    {
        public int status { get; set; }

        public string message { get; set; }

        public string custReference { get; set; }

        public string customerReferenceAlternate { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string otherName { get; set; }

        public string email { get; set; }

        public string phone { get; set; }

        public string thirdPartyCode { get; set; }

        public string agentName { get; set; }

        public string branchName { get; set; }

        public string registrationNo { get; set; }

        public List<ERegInvoicesItem> items { get; set; }

        public Double totalAmount { get; set; }

        public double TransactionFee { get; set; }

    }


    public class PayItem
    {
        public string productName { get; set; }
        public string productCode { get; set; }
        public string quantity { get; set; }
        public string price { get; set; }
        public string subTotal { get; set; }
        public string tax { get; set; }
    }


    public class ERegInvoicesItem
    {
        public string productName { get; set; }
        public string productCode { get; set; }
        public int quantity { get; set; }
        public int price { get; set; }

        public string subTotal { get; set; }

        public string tax { get; set; }

    }

    public class HoldInvoicesViewModel
    {
        public IEnumerable<InvoicesViewModel> HoldAllInvoices { get; set; }
        public Pager Pager { get; set; }
    }

    public class EBuyPowerItem
    {
        public bool error { get; set; }
        public string discoCode { get; set; }
        public string vendType { get; set; }
        public string meterNo { get; set; }
        public int minVendAmount { get; set; }
        public int maxVendAmount { get; set; }
        public int responseCode { get; set; }
        public int outstanding { get; set; }
        public double debtRepayment { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string tariff { get; set; }
        public string orderId { get; set; }

    }

    public class EBuyPowerPayment
    {
        public string phone { get; set; }
        public string paymentType { get; set; }
        public string discoCode { get; set; }
        public string vendType { get; set; }
        public string meterNo { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public int amount { get; set; }
        public string orderId { get; set; }

        //public int ProductItemID { get; set; } 
        public int ProductID { get; set; }

    }


    public class EBuyPowerPaymentPostAPI
    {
        public string meter { get; set; }
        public string disco { get; set; }
        public string vendType { get; set; }

        public string orderId { get; set; }
        public string paymentType { get; set; }
        public string phone { get; set; }

        public string email { get; set; }
        public string name { get; set; }
        public int amount { get; set; }
    }


    public class EBuyPowerQuery
    {
        public string orderId { get; set; }

        public string responseMessage { get; set; }
        public double amountGenerated { get; set; }

        public string disco { get; set; }
        public string receiptNo { get; set; }
        public string tax { get; set; }
        public string vendTime { get; set; }
        public string token { get; set; }
        public double totalAmountPaid { get; set; }
        public string units { get; set; }

        public double vendAmount { get; set; }

        public string vendRef { get; set; }


    }


    //public class EBuyPowerQueryTransaction
    //{ 
    //    public int id { get; set; }
    //    public string amountGenerated { get; set; }
    //    public string disco { get; set; }
    //    public string debtAmount { get; set; }
    //    public string debtRemaining { get; set; }
    //    public string orderId { get; set; }
    //    public string receiptNo { get; set; }
    //    public string tax { get; set; }
    //    public string vendTime { get; set; }
    //    public string token { get; set; }
    //    public int totalAmountPaid { get; set; }
    //    public string units { get; set; }
    //    public string vendAmount { get; set; }
    //    public string vendRef { get; set; }
    //    public int responseCode { get; set; }
    //    public string responseMessage { get; set; }

    //}


    public class Data
    {
        public int id { get; set; }
        public string amountGenerated { get; set; }
        public string disco { get; set; }
        public string debtAmount { get; set; }
        public string debtRemaining { get; set; }
        public string orderId { get; set; }
        public string receiptNo { get; set; }
        public string tax { get; set; }
        public string vendTime { get; set; }
        public string token { get; set; }
        public int totalAmountPaid { get; set; }
        public string units { get; set; }
        public string vendAmount { get; set; }
        public string vendRef { get; set; }
        public int responseCode { get; set; }
        public string responseMessage { get; set; }
    }

    public class Result
    {
        public bool status { get; set; }
        public Data data { get; set; }
    }

    public class Root
    {
        public Result result { get; set; }
    }




    public class shagoAirtime
    {
        public string serviceCode { get; set; }
        public string phone { get; set; }
        public int amount { get; set; }
        public string vend_type { get; set; }
        public string network { get; set; }
        public int request_id { get; set; }

    }

    public class shagoBundle
    {
        //public int amount { get; set; }
        public string serviceCode { get; set; }
        public string network { get; set; }
        public string phone { get; set; }

    }





}
