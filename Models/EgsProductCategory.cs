using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsProductCategory
    {
        [Key]
        public int ProductCategoryID { get; set; }
        public string ProductCategoryCode { get; set; }
        public string ProductCategoryName { get; set; }
        public string FormCategoryType { get; set; }
        public string Image { get; set; }
        public string URL { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string MobileIcon { get; set; }

        [InverseProperty("ProductCategories")]
        public IEnumerable<EgsProduct> Products { get; set; }

        public bool IsActive { get; set; }

    }
}
