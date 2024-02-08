using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class EgsTransactionMethodViewModel
    {
        public int TransactionMethodID { get; set; }
        public string TransactionMethod { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
    }
    
    public class DisplayTransactionMethodViewModel
    {
        public int TransactionMethodID { get; set; }
        public string TransactionMethod { get; set; }
        public DateTime DateCreated { get; set; }
        public IEnumerable<object> Items { get; set; }

    }
    public class HoldDisplayTransactionMethodViewModel
    {
        public IEnumerable<DisplayTransactionMethodViewModel> HoldAllTransactionMethods { get; set; }
        public Pager Pager { get; set; }
    }
    public class TransMethodModelView
    {
        public List<TMListView> items { get; set; }
    }

    public class TMListView
    {
        public string itemName { get; set; }
        public int itemValue { get; set; }
    }
}
