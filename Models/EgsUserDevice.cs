using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsUserDevice
    {
        [Key]

        public int UserDeviceId { get; set; }

        public string DeviceName { get; set; }

        public string DeviceType { get; set; }

        public string IPAddress { get; set; }

        public string UserAgent { get; set; }

        public DateTime DateRegistered { get; set; }

        public int UserId { get; set; }
         
         

        public bool IsActive { get; set; }

        public bool IsEmailSent { get; set; }

        public string RequestID { get; set; }


        public DateTime DateEmailSent { get; set; }
        public bool IsApproved { get; set; }
        public int ApprovedBy { get; set; }
        public string TerminalID { get; set; }
        public DateTime DateApproved { get; set; }









    }
}
