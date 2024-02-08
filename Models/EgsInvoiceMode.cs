using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsInvoiceMode
    {
        [Key]
        public int InvoiceModeID { get; set; }
        public string InvoiceModeType { get; set; }
            
    }
}
