using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway21052021.Models
{
    public class EgsWebhooklogs
    {
        [Key]
        public int webHooklogsID { get; set; }
        public string provider { get; set; }
        public string service { get; set; }
        public string payload { get; set; }
        public DateTime DateCreated { get; set; }
        public string DateModified { get; set; }

    }
}
