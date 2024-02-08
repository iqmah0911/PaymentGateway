using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsInsuranceRemittance
    {

        [Key]
        public int InsuranceID { get; set; }

        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public string ProductItem { get; set; }
        public double Premuim { get; set; }
        public double CBS { get; set; }
        public double NIA { get; set; }
        public double EGOLEPAY { get; set; }
        public double NIAMEMBER { get; set; }
        public double BROKER { get; set; }

        public DateTime SettlementDate { get; set; }
        public DateTime TransactionDate { get; set; }

        public string CompanyName { get; set; }
        public string AccountAlias { get; set; }
        public string ReceivingBank { get; set; }

        public string AccountNumber { get; set; }

        public string TransactionReference { get; set; }

        public string BankCode { get; set; }
        public string RegNumber { get; set; }
        public string PolicyNumber { get; set; }


        public bool IsTransferred { get; set; }

        public bool IsSettled { get; set; }
        public DateTime TransferDate { get; set; }

        public int CreatedBy { get; set; }

        public double ECOWAS { get; set; }
        public double LAGOS { get; set; }

        //
        public double INSURER { get; set; }

    }
}
