using Microsoft.AspNetCore.Mvc.Rendering;
using PaymentGateway21052021.Areas.Invoices.Models;
using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class OrdersModel : EInvoicesViewModel
    {
        public int ProductCategoryID { get; set; }
        public int ProductItemID { get; set; }
        public string ProductItemName { get; set; }
        public string ProductItemCode { get; set; }
        public int ProductID { get; set; }
        //public string ProductName { get; set; }
        public int InvoiceID { get; set; }
        public int ProductModeID { get; set; }
        public double AmountRate { get; set; }
        public double Amount { get; set; }
        public string PhoneNumber { get; set; }
        //public string Email { get; set; }
        public string RegNumber { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime DateCreated { get; set; }
        public int Message { get; set; }
        public string MeterNo { get; set; } 
        public string discoCode { get; set; } 
        public string vendType { get; set; }
        public string LoginUser { get; set; }
        public string UserState { get; set; }
        public bool orderId => true;
        public string GeneratedBy { get; set; }
        public string ServiceType { get; set; }
        public string AccountNo { get; set; } 
        public int NumberofPins { get; set; }

        public string StatusMessage { get; set; }

        public string QuantityAvailable { get; set; }
    }

    public class InvoicePayModel
    {
        public int productID { get; set; }
        public string Email { get; set; }
        public string refno { get; set; }
        public string ProductItemCode { get; set; }
        public int ProductItemID { get; set; }
        public double TotalAmount { get; set; }
        public string IPAddress { get; set; }
    }


    public class ReprintModel
    { 
        public string Email { get; set; }
        public string refno { get; set; }
        public string Name { get; set; }
        public string RegistrationNo { get; set; }
        public double Amount { get; set; }
        public double AmountRate { get; set; }
        public double TotalAmount { get; set; }
        public string GeneratedBy { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionTime { get; set; }

    }



    public class UCardService
    {
        public string Url { get; set; }
        public bool IsEnabled { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class UCardDataService
    {
        public string Url { get; set; }
        public bool IsEnabled { get; set; } 
    }

    public class ShagoPayService
    {
        public string Url { get; set; }
        public string testUrl { get; set; }
        public string hashKey { get; set; } 
        public string email { get; set; }
        public string password { get; set; } 
        public bool IsEnabled { get; set; }
    }


    public class ITextService
    {
        public string wallet { get; set; }
        public string username { get; set; } 
        public string password { get; set; }
        public string identifier { get; set; }
        public bool IsEnabled { get; set; }
         
    }



    public class EgolePayService
    {
        public string Url { get; set; }
        public string stageUrl { get; set; }
        public bool IsEnabled { get; set; }
    }


    public class AutoRegNotificationService
    {
        public string Url { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class NotificationService
    {
        public string Url { get; set; }
        public bool IsEnabled { get; set; }
    }


    public class OrderDetailsModel
    {
        public int ProductItemID { get; set; }
        public string ProductItemName { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int InvoiceID { get; set; }
        public int ProductModeID { get; set; }
        public double AmountRate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string RegNumber { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime DateCreated { get; set; }

        public string meter { get; set; }

        public string disco { get; set; }

        public string vendType { get; set; }

        public bool orderId => true;  

    }
     
    public class HomeStoreModel
    {
        public int ProductCategoryID { get; set; }
        public string ProductCategoryCode { get; set; }
        public string ProductCategoryName { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string searchText { get; set; }
        public int ProductItemRateID { get; set; }
        public int ProductItemID { get; set; }
        public string ProductItemName { get; set; }
        public double AmountRate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string RegNumber { get; set; }
        public bool IsFixedAmount { get; set; }
        public int InvoiceModeId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; } //
        public string ReferenceNo { get; set; }
        public int InvoiceID { get; set; }
    }

    public class ProductWalletViewModel
    {
        public int ProductItemID { get; set; }
        public string ProductItemName { get; set; }
        public double AmountRate { get; set; }
        public string RegNumber { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class PayWithWalletViewModel
    {
        public int InvoiceID { get; set; }
        public int WalletID { get; set; }
        public int UserID { get; set; }
        public int WalletTransactionID { get; set; }
        public int TransactionTypeID { get; set; }
        public int TransactionMethodID { get; set; }
        public int ProductItemID { get; set; }
        public string ProductItemName { get; set; }
        public string ProductName { get; set; }
        public string TransactionMethodName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string RegNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public string ReferenceNo { get; set; }
        public string WalletAccountNumber { get; set; }
        public double Amount { get; set; }
    }

    public class HoldProductsAlike : HomeStoreModel
    {
        public IEnumerable<HomeStoreModel> HoldAllProductsAlike { get; set; }
        public Pager Pager { get; set; }
    }

    public class BuyPowerViewModel
    { 
        public string orderId { get; set; }
        public string meter { get; set; }
        public string disco { get; set; } 
        public string phone { get; set; }

        public string paymentType { get; set; }
        public string vendType { get; set; }
        public int amount { get; set; }

        public string email { get; set; }

        public string name { get; set; }
    }


    public class Service
    {
        public string amount { get; set; }
        public string description { get; set; }
        public string item_id { get; set; }
        public string item { get; set; }
        public string category_id { get; set; }
        public string category { get; set; }
    }

   
    public class NotificationRoot
    {
        public string total { get; set; }
        public string date { get; set; }
        public string source { get; set; }
        public string customer_id { get; set; }
        public string hash { get; set; }
        public string passKey { get; set; }
        //public List<PayItem> services { get; set; }
        public List<Service> services { get; set; } 
    }


     

    public static class StaticDrpDwnListItems
    {
        public static List<SelectListItem> VendTypeList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem {Text = "PREPAID", Value = "PREPAID"},
                new SelectListItem {Text = "POSTPAID", Value = "POSTPAID"}
            };
        }

        public static List<SelectListItem> paymentTypeList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem {Text = "ONLINE", Value = "ONLINE"}
                
            };
        }

        public static List<SelectListItem> DiscoCodeList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem {Text = "ABUJA", Value = "ABUJA"},
                new SelectListItem {Text = "KADUNA", Value = "KADUNA"},
                new SelectListItem {Text = "JOS", Value = "JOS"},
                new SelectListItem {Text = "IKEJA", Value = "IKEJA"},
                new SelectListItem {Text = "EKO", Value = "EKO"},
                 new SelectListItem {Text = "KANO", Value = "KANO"},
                     new SelectListItem {Text = "ENUGU", Value = "ENUGU"},
                 new SelectListItem {Text = "PH", Value = "PH"}
            };
        }


        public static List<SelectListItem> DocumentTypeList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem {Text = "PDF", Value = "PDF"},
                new SelectListItem {Text = "JPG", Value = "JPG"},
                new SelectListItem {Text = "WORD", Value = "WORD"}
              
            };
        }



    }
    //VFDTransactionService

    public class VFDTransferService
    {
        public string Url { get; set; }
        public bool IsEnabled { get; set; }
        public string token { get; set; }
        public string walletCredentials { get; set; }
    }


    public class UCardCableService
    {
        public bool IsEnabled { get; set; }
        public string clientId { get; set; }
        public string clientSecret { get; set; }
        public string fullName { get; set; }
        public string transactionAuth { get; set; }
         
    }


}
