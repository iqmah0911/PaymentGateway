using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsChannels
    {
        [Key] 
        public int ChannelID { get; set; }
         
        public string Channel { get; set; }
     
        public int status { get; set; }
        public int priority { get; set; }
        public double upperTransactionBound { get; set; }

        public double lowerTransactionBound { get; set; }

        public double chargeValue { get; set; }

        public int chargeIsFlat { get; set; }
        public int cap { get; set; }

        public string secretKey { get; set; }

        public int EnumChannelID { get; set; }
    }



}
