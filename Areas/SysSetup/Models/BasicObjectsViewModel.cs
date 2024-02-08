using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class BasicObjectsViewModel
    {
        public DateTime DateCreated { get; set; }
        public int TransCount { get; set; }
        public int CreatedBy { get; set; }
        public string WalletInfo { get; set; }
        public string Business { get; set; }
        public string InvRefNum { get; set; }
        public string PayRefNum { get; set; }
        public string ItemCategory { get; set; }
        public double Amount { get; set; }
        public double ComAmount { get; set; }
    }
}
