using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsTicketDetails
    {

        [Key]
        public int TDetailsID { get; set; }

        [ForeignKey("TicketID")]
        public EgsTickets Tickets { get; set; }

        public string Feedback { get; set; }

        public int FeedbackBy { get; set; }
        public Boolean IsOfficial { get; set; }

        public DateTime DateCreated { get; set; }

    }
}
