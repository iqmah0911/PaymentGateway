using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsTickets
    {

        [Key]
        public int TicketID { get; set; }

        public string ReferenceNo { get; set; }

        public string Subject { get; set; }
        public string Description { get; set; }

        [ForeignKey("WalletID")]
        public EgsWallet Wallet { get; set; }

        public int ReceivedBy { get; set; }
        public int TreatedBy { get; set; }
        public DateTime DateCreated { get; set; }

        public string Status { get; set; }





    }
}
