using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsUserNotification
    {
        [Key]
        public int UserNotificationID { get; set; }

        public string Service { get; set; }


        public bool IsDeposit { get; set; }

        public bool IsWithdrawal { get; set; }

        public bool IsCommission { get; set; }

        public bool IsMoved { get; set; }


        public int UserID { get; set; }


        public int CreatedBy { get; set; }






    }
}
