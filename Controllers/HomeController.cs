using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.ProductCategory = ProductCategoryList();
            return View();
        }

        public class ProductModel
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public DateTime DateCreated { get; set; }
            public int CreatedBy { get; set; }
            public int ProductCategoryID { get; set; }
            public string ProductCategory { get; set; }

        }

        public IActionResult Privacy(string loginType = null)
        {
            //TempData["loginType"] = loginType;
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string ProductName, string ProductId)
        {
            ViewBag.ProductCategory = ProductCategoryList();
            //ViewBag.Message = "ProductName: " + ProductName + " ProductId: " + ProductId;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        //Method to pull ProductCategories
        public async Task<List<EgsProductCategory>> ProductCategoryList()
        { 
            var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

            string Baseurl = APIServiceConfig?.Url;

            List<EgsProductCategory> prodCategoryList = new List<EgsProductCategory>();

            using (var client = new HttpClient())
            {
                string IResponse = General.MakeRequest(Baseurl + "api/ProductCategory/", "", "GET");

                string responseToCaller = string.Empty;

                if (!String.IsNullOrEmpty(IResponse))
                {
                    prodCategoryList = JsonConvert.DeserializeObject<List<EgsProductCategory>>(IResponse);
                    //responseToCaller = "sent";
                    prodCategoryList.Insert(0, new EgsProductCategory { ProductCategoryID = 0, ProductCategoryName = "--Select Product Category --" });
                }
                else
                {
                    //responseToCaller = "not sent";
                    prodCategoryList.Insert(0, new EgsProductCategory { ProductCategoryID = 0, ProductCategoryName = "--Select Product Category --" });
                }

            }

            return prodCategoryList;
        }

    }
}
