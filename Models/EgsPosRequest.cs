using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace PaymentGateway21052021.Models
{
    public class EgsPosRequest
    {
        [Key]
        public int PosrequestID { get; set; }

        [ForeignKey("PosTypeID")]
        public EgsPosType PosType { get; set; }

        public string MainWalletAccount { get; set; }
        public double Amount { get; set; }

        public string Devicetype { get; set; }

        public string Status { get; set; }

        public int UserID { get; set; }

        public string Comment { get; set; }

        public DateTime DateCreated { get; set; }
        public bool IsProccessed { get; set; }
        public bool IsApproved { get; set; }
        public int Approvedby { get; set; }

        public DateTime DateApproved { get; set; }

        public int RejectedBy { get; set; }

        public DateTime DateRejected { get; set; }


    }
}
