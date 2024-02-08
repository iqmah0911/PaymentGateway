using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using PaymentGateway21052021.Helpers; 
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class ApplicationNotifications
    {
    }

    public class VFDResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public string data { get; set; }

    }

    public class TransferRespData
    {
        public string txnId { get; set; }
        public string sessionId { get; set; }
        public string reference { get; set; }
    }

    public class VFDTransferResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public TransferRespData data { get; set; }
    }

    public class VFDEnquiryReponse
    {
        public string accountNo { get; set; }
        public string accountBalance { get; set; }
        public string accountId { get; set; }
        public string client { get; set; }
        public string clientId { get; set; }
        public string savingsProductName { get; set; }
    }

    public class VFDAccountEnquiry
    {
        public string status { get; set; }
        public string message { get; set; }
        public VFDEnquiryReponse data { get; set; }
    }

    public class Account
    {
        public string number { get; set; }
        public string id { get; set; }
    }

    //public class VFDAccountEnquiryData
    //{
    //    public string accountNo { get; set; }
    //    public string accountBalance { get; set; }
    //    public string accountId { get; set; }
    //    public string client { get; set; }
    //    public string clientId { get; set; }
    //    public string savingsProductName { get; set; }
    //}

    //public class VFDAccountEnquiry
    //{
    //    public string status { get; set; }
    //    public string message { get; set; }
    //   // public VFDAccountEnquiryData data { get; set; }
    //}


    public class VFDRecipientResponse
    {
        public string name { get; set; }
        public string clientId { get; set; }
        public string bvn { get; set; }
        public Account account { get; set; }
        public string status { get; set; }
        public string currency { get; set; }
        public string bank { get; set; }
    }

    public class VFDRecipient
    {
        public string status { get; set; }
        public string message { get; set; }
        public VFDRecipientResponse data { get; set; }
    }


    public class VFDTransferData
    {
        public string fromSavingsId { get; set; }
        public string amount { get; set; }
        public string toAccount { get; set; }
        public string fromBvn { get; set; }
        public string signature { get; set; }
        public string fromAccount { get; set; }
        public string toBvn { get; set; }
        public string remark { get; set; }
        public string fromClientId { get; set; }
        public string fromClient { get; set; }
        public string toKyc { get; set; }
        public string reference { get; set; }
        public string toClientId { get; set; }
        public string toClient { get; set; }
        public string toSession { get; set; }
        public string transferType { get; set; }
        public string toBank { get; set; }
        public string toSavingsId { get; set; }
    }

    public class NotificationResponse
    {
        /// <summary>
        /// This shows the success status of request. The value for sccess is 0.
        /// </summary>
        public string StatusCode { get; set; }
        public string Message { get; set; }
        public string TransactionDate { get; set; } = null;
        /// <summary>
        /// This is the customer_id reference
        /// </summary>
        [JsonProperty("customer_id")]
        public string Reference { get; set; }
    }

    public class NotificationData
    {
        /// <summary>
        /// 
        /// </summary>
        public int MerchantId { get; set; }
        public string MerchantCode { get; set; }
        public string UniqueReferenceNo { get; set; } 
        public string CustomerAlternateRef { get; set; }
        public string cusName { get; set; }
        public string cusEmail { get; set; }
        public string cusPhoneNo { get; set; }
        public string Product { get; set; }
        public string ProductCode { get; set; }
        public double TransactionAmount { get; set; }
        //public List<ProductsItemModel> ProductItemLists { get; set; }
        public string DateCreated { get; set; }
        public List<PayItem> paymentItemsList { get; set; }
        public double TransactionFee { get; set; }

    }

    public class ProductNotificationResponse : InvoicesResponseModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int MerchantId { get; set; }
        public string MerchantCode { get; set; }
        public string UniqueReferenceNo { get; set; }
        public string cusName { get; set; }
        public string cusEmail { get; set; }
        public string cusPhoneNo { get; set; }
        public string Product { get; set; }
        public string ProductCode { get; set; }
        public double TransactionAmount { get; set; }
        public List<ProductsItemModel> ProductItemLists { get; set; }
        public string DateCreated { get; set; }

    }

    public class ProductsItemModel
    {
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
    }

    public class InvoicesResponseModel
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

        public List<ProductsItemModel> items { get; set; }

        public int totalAmount { get; set; }
    }

    //AutoReg Notification
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class PayItem
    {
        public string productName { get; set; }
        public string productCode { get; set; }
        public string quantity { get; set; }
        public string price { get; set; }
        public string subTotal { get; set; }
        public string tax { get; set; }
    }

    public class PaymentItems
    {
        public List<PayItem> items { get; set; }
    }

    public class CustomerInformationResponse
    {
        public string status { get; set; }
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
        //public List<PayItem> paymentItems { get; set; }
        public PaymentItems paymentItems { get; set; }
        public string totalAmount { get; set; }
        public List<PayItem> items { get; set; }

    }

    public class AutoRegResponse
    {
        public CustomerInformationResponse customerInformationResponse { get; set; }
        public string status { get; set; }
        public string message { get; set; }
    }


    public class CapayResponse
    {
        public string status { get; set; }
        public string message { get; set; }

    }

    public class InvoiceReportResponse
    {
        public string WalletInfo { get; set; }
        public string Business { get; set; }
        public string AlternateReferenceNo { get; set; }
        public string ServiceNumber { get; set; }
        public string ProductItemCategory { get; set; }
        public string productName { get; set; }
        public string productItemName { get; set; }
        public string PaymentReference { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime DateCreated { get; set; }
        public int TransCount { get; set; }
        public double Amount { get; set; }
        public double InvAmount { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public double Commisionamount { get; set; }
        public bool IsAggregatorSettled { get; set; }
        public string AccountInfo { get; set; }
        public bool AggregatorPushed { get; set; }
        public double CommissionAmount { get; set; }
        public int AggregatorID { get; set; }
        public string CommissionPeriod { get; set; }
    }


    public class LReportresponse
    {
        public List<InvoiceReportResponse> invreportreponse { get; set; }
    }

    public class HoldRPTInvoiceViewModel
    {
        public IEnumerable<InvoiceReportResponse> HoldRPTInvoices { get; set; }
        public Pager Pager { get; set; }
    }


    public class AutoDResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string custReference { get; set; }
        public int productModeID { get; set; }
        public string referenceNo { get; set; }
        public string registrationNo { get; set; }
        public string productName { get; set; }
        public double amountRate { get; set; }
        public double amount { get; set; }
        public double totalAmount { get; set; }
        public int productID { get; set; }
        public int productItemID { get; set; }
        public string productItemCode { get; set; }

    }

    public class FundPaymentResponse
    {
        public string transactionreference { get; set; }
        public string status { get; set; }
        public string message { get; set; }
    }

    public class CommissionPushResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string TotalAmount { get; set; }

    }

    public class AirtimeRechargeResponse
    {
        public string status { get; set; }
        public string message { get; set; }

        public string requestID { get; set; }

    }

     
    public class AirtimeResponse
    {
        public int amount { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public string transId { get; set; }
        public string date { get; set; }
        public string phone { get; set; }
        public string type { get; set; }
    }










}
