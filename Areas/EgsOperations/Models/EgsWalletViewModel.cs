using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.EgsOperations.Models
{
   
        public class EgsWalletViewModel
        {

        }

        public class WalletViewModel
        {
            public int WalletID { get; set; }
            public string WalletAccountNumber { get; set; }
            public DateTime DateCreated { get; set; }
            public int CreatedBy { get; set; }
        }

    public class FundWalletViewModel
    {
        //public int WalletID { get; set; }
        public int TransactionMethodID { get; set; }
        public double Amount { get; set; }
        public DateTime TransactionDate { get; set; }

        //public int CreatedBy { get; set; }
    }

    public class FundWalletAdminViewModel
    {
        public int UserID { get; set; }
        public double Amount { get; set; }
        public string TransactionRef { get; set; }
        public string Narration { get; set; }
        public DateTime TransactionDate { get; set; }
        public string WalletAccountNumber { get; set; }

        //public int CreatedBy { get; set; }
    }

    public class FundWalletNotificationViewModel
    {
        /// <summary>
        /// This is the initiator’s account number
        /// </summary>
        public string walletAccountnumber { get; set; }
        /// <summary>
        /// This is the transaction amount
        /// </summary>
        public double Amount { get; set; }
        /// <summary>
        /// This is the transaction reference.
        /// </summary>
        public string TransactionReference { get; set; }
        /// <summary>
        /// This is the transaction narration.
        /// </summary>
        public string Narration { get; set; }

        /// <summary>
        /// DateTime of transaction.
        /// </summary>
        public string TimeStamp { get; set; }

        public string ipAddress { get; set; }
        //
        public int CreatedBy { get; set; }

    }
    public class UpdateWalletViewModel
        {
            public int WalletID { get; set; }
            public string WalletAccountNumber { get; set; }
            public DateTime DateCreated { get; set; }
            public bool IsActive { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }

    }

        public class DisplayWalletViewModel
        {
            public int WalletID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string WalletAccountNumber { get; set; }
            public bool IsActive { get; set; }
            public DateTime DateCreated { get; set; }
            public Pager Pager { get; set; }
            public IEnumerable<object> Items { get; set; }
        }

        public class HoldDisplayWalletViewModel
        {
            public IEnumerable<DisplayWalletViewModel> HoldAllWallet { get; set; }
            public Pager Pager { get; set; }
        }
    
}
