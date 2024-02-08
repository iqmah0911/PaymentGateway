using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway21052021.Models
{
    public class EgsAccountManagers
    {
        [Key]
        public int accountManagerID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string stateID { get; set; }
        public string email { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateDeleted { get; set; }


    }
}
