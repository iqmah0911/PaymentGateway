using System.ComponentModel.DataAnnotations;
using System;

namespace PaymentGateway21052021.Models
{
    public class EgsPosType
    {
        [Key]
        public int PosTypeID { get; set; }

        public string PosType { get; set; }

        public double Amount { get; set; }

        public DateTime DateCreated { get; set; }

        public int CreatedBy { get; set; }

        public bool IsAvailaible { get; set; }

    }
}
