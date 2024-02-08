using System.ComponentModel.DataAnnotations;
using System;

namespace PaymentGateway21052021.Models
{
    public class EgsTRSRemittance
    {
        [Key]
        public int TRSID { get; set; }

        public double Amount { get; set; }

        public DateTime RemittanceDate { get; set; }

        public string CreatedByFullName { get; set; }

        public int CreatedByID { get; set; }

        public DateTime DateCreated { get; set; }

        public string ReferenceNo { get; set; }

    }
}
