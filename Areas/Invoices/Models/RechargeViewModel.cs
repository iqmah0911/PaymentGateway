using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.Invoices.Models
{
    public class RechargeViewModel
    {
        public string msisdn { get; set; }
        public int amount { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string hash { get; set; }

    }

    public class Datum
    {
        public int id { get; set; }
        public string SKU { get; set; }
        public string country { get; set; }
        public string Operator { get; set; }
        public string data_amount { get; set; }
        public string data_validity { get; set; }
        public string topup_price { get; set; }
        public string topup_currency { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string deleted_at { get; set; }
        public string display_format { get; set; }
        public int provider_switch { get; set; }
        public string product_id1 { get; set; }
        public string allowance1 { get; set; }
        public string price1 { get; set; }
        public string validity2 { get; set; }
        public int is_exposed { get; set; }
    }

    public class DataRoot
    {
        public string code { get; set; }
        public IEnumerable<Datum> data { get; set; }
        public string product_Id { get; set; }
        public string msisdn { get; set; }
        public string phone { get; set; }
        public int Amount { get; set; }

        public int ProductModeID { get; set; }

        public int ProductID { get; set; }
    }

    public class DataBundle
    {
        public string hash { get; set; }
        public string product_id { get; set; }
        public string msisdn { get; set; }
    }

    public class VendingCheck
    {
        public int code { get; set; }
        public string description { get; set; }
    }

    public class VendingResponse
    {
        public int amount { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public string transId { get; set; }
        public string date { get; set; }
        public string phone { get; set; }
        public string package { get; set; }
    }


    public class RVendResponse
    {
        public int amount { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public string transId { get; set; }
        public string date { get; set; }
        public string phone { get; set; }
        public string type { get; set; }
    }

     

    public class Product
    {
        //public int price { get; set; }
        public double price { get; set; }
        public string code { get; set; }
        public string allowance { get; set; }
        public string validity { get; set; }
    }

    public class Roots
    {
        public string message { get; set; }
        public string status { get; set; }
        public IEnumerable<Product> product { get; set; }
        public string phone { get; set; }
        public string network { get; set; }
        public string product_Id { get; set; }
        public int Amount { get; set; }
        public int ProductModeID { get; set; }
        public int ProductID { get; set; }

        public string serviceCode { get; set; }
        public string bundle { get; set; }
        public int request_id { get; set; }
    }

    public class ShagoBundle
    {
        public string serviceCode { get; set; }
        public string phone { get; set; }
        public string amount { get; set; }
        public string bundle { get; set; }
        public string network { get; set; }
        public string package { get; set; }
        public string request_id { get; set; }
    }

    public class ShagoBundleResponse
    {
        public int amount { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public string transId { get; set; }
        public DateTime date { get; set; }
        public string phone { get; set; }
        public string package { get; set; }

    }

    public class ITextElectricity
    {
        public string meterNo { get; set; }
        public string accountType { get; set; }
        public string service { get; set; }
        public string amount { get; set; }
        public string channel { get; set; }

    }

    public class ITextTokenRequest
    {
        public string wallet { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string identifier { get; set; }

    }

    public class ITextValidationmodel
    {
        public string vendType { get; set; }
        public string discoCode { get; set; }
        public string Amount { get; set; }
        public string MeterNo { get; set; }
        public int ProductID { get; set; }
    }

    public class IBEDCPaymodel
    { 
        public string AccountNo { get; set; }
        public string Amount { get; set; }
        public string MeterNo { get; set; }
        //public int ProductID { get; set; }
    }


    public class ITextPaymentmodel
    {
        public string customerPhoneNumber { get; set; }
        public string paymentMethod { get; set; }
        public string service { get; set; }
        public string clientReference { get; set; }
        public string pin { get; set; }
        public string productCode { get; set; }
        public int userid { get; set; }
        public int ProductID { get; set; }
        public int Amount { get; set; }

        public string card { get; set; }

    }

    public class ITextPaymentResponse
    {
        public bool error { get; set; }
        public string account { get; set; }
        public string name { get; set; }
        public string token { get; set; }
        public string accountNumber { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string minimumPurchase { get; set; }
        public string businessUnit { get; set; }
        public string businessUnitId { get; set; }
        public string undertaking { get; set; }
        public string customerArrears { get; set; }
        public string tariffCode { get; set; }
        public string tariff { get; set; }
        public string paidamount { get; set; }
        public string merchantId { get; set; }
        public string recieptNumber { get; set; }
        public DateTime transactionDate { get; set; }
        public string transactionReference { get; set; }
        public string transactionStatus { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string externalReference { get; set; }
        public string type { get; set; }
        public string units { get; set; }
        public string vat { get; set; }
        public int costofunit { get; set; }
        public DateTime lastTransactionDate { get; set; }
        public string responseCode { get; set; }
        public int amount { get; set; }
        public string reference { get; set; }
        public string sequence { get; set; }
        public string clientReference { get; set; }
    }



    public class ITexPayData
    {
        public bool error { get; set; }
        public string account { get; set; }
        public string name { get; set; }
        public string token { get; set; }
        public string accountNumber { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string minimumPurchase { get; set; }
        public string businessUnit { get; set; }
        public string businessUnitId { get; set; }
        public string undertaking { get; set; }
        public string customerArrears { get; set; }
        public string tariffCode { get; set; }
        public string tariff { get; set; }
        public int paidamount { get; set; }
        public string merchantId { get; set; }
        public string recieptNumber { get; set; }
        public DateTime transactionDate { get; set; }
        public string transactionReference { get; set; }
        public string transactionStatus { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string externalReference { get; set; }
        public string type { get; set; }
        public string units { get; set; }
        public string vat { get; set; }
        public double costofunit { get; set; }
        public DateTime lastTransactionDate { get; set; }
        public string responseCode { get; set; }
        public int amount { get; set; }
        public string reference { get; set; }
        public string sequence { get; set; }
        public string clientReference { get; set; }
    }




    public class EData
    {
        public bool error { get; set; }
        public string message { get; set; }
        public string customerId { get; set; }
        public string name { get; set; }
        public string meterNumber { get; set; }
        public string accountNumber { get; set; }
        public string businessUnit { get; set; }
        public string businessUnitId { get; set; }
        public string undertaking { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public DateTime lastTransactionDate { get; set; }
        public string minimumPurchase { get; set; }
        public string customerArrears { get; set; }
        public string tariffCode { get; set; }
        public string tariff { get; set; }
        public string description { get; set; }
        public string customerType { get; set; }
        public string responseCode { get; set; }
        public string productCode { get; set; }
        public string service { get; set; }
        public string Amount { get; set; }
        public string ProductID { get; set; }
    }

    public class ERoot
    {
        public string responseCode { get; set; }
        public string message { get; set; }
        public EData data { get; set; }
    }


    //Cable tv models
    public class Cablemodel
    {
        public string responseCode { get; set; }
        public string message { get; set; }
        public EData data { get; set; }
    }

     
    public class RawOutput
    {
        public string accountStatus { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string customerType { get; set; }
        public int invoicePeriod { get; set; }
        public DateTime dueDate { get; set; }
        public int customerNumber { get; set; }
    }

    public class Item
    {
        public string code { get; set; }
        public int price { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class CurrentBouquetRaw
    {
        public int amount { get; set; }
        public List<Item> items { get; set; }
    }

    public class CableNResponse
    {
        public string name { get; set; }
        public string address { get; set; }
        public int outstandingBalance { get; set; }
        public DateTime dueDate { get; set; }
        public string district { get; set; }
        public string accountNumber { get; set; }
        public string minimumAmount { get; set; }
        public RawOutput rawOutput { get; set; }
        public string errorMessage { get; set; }
        public bool hasDiscount { get; set; }
        public CurrentBouquetRaw currentBouquetRaw { get; set; }
        public string currentBouquet { get; set; }
        //custom property
        public int status { get; set; }
    }

    public class CableValidation
    {
        public string name { get; set; }
        public string address { get; set; }
        public int outstandingBalance { get; set; }
        public DateTime dueDate { get; set; }
        public string district { get; set; }
        public string accountNumber { get; set; }
        public string minimumAmount { get; set; }
        public RawOutput rawOutput { get; set; }
        public string errorMessage { get; set; }
        public bool hasDiscount { get; set; }
        public CurrentBouquetRaw currentBouquetRaw { get; set; }
        public string currentBouquet { get; set; }
        //custom property
        public int status { get; set; }
    }

    public class AvailablePricingOption
    {
        public int monthsPaidFor { get; set; }
        public int price { get; set; }
        public int invoicePeriod { get; set; }
    }

    public class NCableDatum
    {
        public List<AvailablePricingOption> availablePricingOptions { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public List<NCableDatum> data { get; set; }
        //Custom Properties
        public string account_ID { get; set; }
        public string msisdn { get; set; }
          
        public double amount { get; set; }

        public string product_id { get; set; }

        public int product_monthsPaidFor { get; set; }

        public string service_type { get; set; }

        public string trans_id { get; set; }

        public string email { get; set; }
        public string prdID { get; set; }
    }

    public class UCabletRootReceipt
    {
        public string message { get; set; }
        public string statusCode { get; set; }
        public string subscriberId { get; set; }
        public string transactionId { get; set; }
        public int amount { get; set; }
        public string providerCode { get; set; }
        public string narration { get; set; }
        public string accountName { get; set; }

    }


    public class NCableInput
    {
        public string msisdn { get; set; }
        public string account_id { get; set; }
        public double amount { get; set; } 
        public string product_id { get; set; }
        public int product_monthsPaidFor { get; set; } 
        public string service_type { get; set; }
        public string trans_id { get; set; }
        public string email { get; set; }
        public string userid { get; set; }
        public int ProductID { get; set; }
    }


    public class NCardCablePayInput
    {
        public string transactionId { get; set; }
        public string subscriberId { get; set; }
        public string productCode { get; set; }
        public string providerCode { get; set; }

        public string transactionPin { get; set; }
        public int amount { get; set; }
        public string addOnCode { get; set; }

        public int quantity { get; set; }
        public string narration { get; set; }
        public string accountName { get; set; }
        public string userid { get; set; }
        public int ProductID { get; set; }
    }


    //Shago WAEC


    public class SWaecProduct
    {
        public int price { get; set; }
        public int availableCount { get; set; }
    }

    public class SShagoWaecRoot
    {

        public string message { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public List<SWaecProduct> product { get; set; }
    }


    public class SPinWResponse
    {
        public string pin { get; set; }
        public string serial { get; set; }
        public string expirydat { get; set; }
        public int amount { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public string transId { get; set; }
        public string date { get; set; }
    }









}
