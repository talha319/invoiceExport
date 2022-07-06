using InvoiceExportPDF.Data;
using InvoiceExportPDF.Models;
using InvoiceExportPDF.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using Nancy.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Mvc.Routing;

namespace InvoiceExportPDF.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly InvoiceExportDBContext _context;

        public InvoiceController(InvoiceExportDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Product> proList = new List<Product>();
            proList = (from c in _context.Products select c).ToList();
            proList.Insert(0, new Product { Id = 0, Name = "--Select Product Name--" });
            ViewBag.Product = proList;
            return View();
        }

        [HttpGet]
        public IActionResult InvoiceProcess(string BussinessDays, string HolidaysDays, string Products)
        {
            try
            {
                int InvoiceNumber = 1;
                decimal invoiceAmount = 0;
                Random random = new Random();
                int invoiceQty = 0;
                
                int indexRandom = 3;

                List<ProductsDetailsInvoice> inv = new List<ProductsDetailsInvoice>();
                decimal TotalBillAmount = 0;
                int TotalInvoiceQuantity = 0;
                List<ProductsDetails> productListInvoice = new List<ProductsDetails>();

                var productListJson = JsonConvert.DeserializeObject<JToken>(Products);
                List<ProductsSaleData> productList = productListJson.ToObject<List<ProductsSaleData>>();

                var holidaysListJson = JsonConvert.DeserializeObject<JToken>(HolidaysDays);
                List<DateTime> holidays = holidaysListJson.ToObject<List<DateTime>>();

                var workingDaysListJson = JsonConvert.DeserializeObject<JToken>(BussinessDays);
                List<DateTime> workingDaysList = workingDaysListJson.ToObject<List<DateTime>>();



                foreach (var item in productList)
                {
                    string[] productDetails = item.ProductID.Split(',');
                    int productID = Convert.ToInt32(productDetails[0]);
                    decimal priceProduct = Convert.ToDecimal(productDetails[1]);
                    decimal GstTexProduct = Convert.ToDecimal(productDetails[2]);
                    int productQuantity = Convert.ToInt32(item.Quantity);

                    decimal poductPriceWithTex = Convert.ToInt32(item.Quantity) * (priceProduct + GstTexProduct);

                    productListInvoice.Add(new ProductsDetails
                    {
                        Id = productID,
                        Name = item.ProductName,
                        Quantity = Convert.ToInt32(item.Quantity),
                        Gsttex = GstTexProduct,
                        Price = priceProduct,
                        RemainingQuantity = productQuantity
                    });
                    TotalInvoiceQuantity += Convert.ToInt32(item.Quantity);
                    TotalBillAmount += poductPriceWithTex;
                }

                while (TotalBillAmount > 0)
                {
                    int i = 1;

                    int productCount = productListInvoice.Count;
                    
                  
                    //while (invoiceAmount < 50000)
                    //{

                    while (i <= productCount)
                    {
                        ProductsDetailsInvoice productsDetailsInvoice = new ProductsDetailsInvoice();
                        
                       

                        int qtySelect = 1;
                        if (invoiceAmount < 50000)
                        {
                            if (productListInvoice[i - 1].RemainingQuantity > 0)
                            {

                                decimal itemAmount = qtySelect * (productListInvoice[i - 1].Price + productListInvoice[i - 1].Gsttex);

                                if (invoiceAmount + itemAmount <= 50000)
                                {
                                    invoiceAmount += itemAmount;
                                    invoiceQty += qtySelect;
                                    TotalInvoiceQuantity -= qtySelect;
                                    // add prodct info with Invoice ID into Invoice Array

                                    productsDetailsInvoice.Id = productListInvoice[i - 1].Id;
                                    productsDetailsInvoice.Name = productListInvoice[i - 1].Name;
                                    productsDetailsInvoice.Quantity = qtySelect;
                                    productsDetailsInvoice.Price = itemAmount;
                                    productsDetailsInvoice.InvoiceNumber = InvoiceNumber;
                                    DateTime randomDate = workingDaysList[indexRandom];
                                    productsDetailsInvoice.DateInvoice = randomDate;
                                    inv.Add(productsDetailsInvoice);
                                    //deduct product remaining qty
                                    productListInvoice[i - 1].RemainingQuantity -= qtySelect;
                                }
                                else
                                {

                                    indexRandom = random.Next(workingDaysList.Count);
                                    TotalBillAmount -= invoiceAmount;
                                    InvoiceNumber = InvoiceNumber + 1;
                                    invoiceAmount = 0;

                                    Debug.WriteLine(TotalBillAmount);
                                }

                            }


                            if (i == productCount && invoiceAmount < 50000 && productListInvoice[i - 1].RemainingQuantity > 0)
                            {
                                i = 1;
                            }
                            if(TotalInvoiceQuantity ==0)
                            {
                                TotalBillAmount -= invoiceAmount;
                                i = productCount+1;
                            }
                            else
                            {
                                i++;
                            }
                            //  TotalBillAmount -= invoiceAmount;
                            //Debug.WriteLine(TotalBillAmount);
                            //if (TotalBillAmount <= 0)
                            //{
                            //    break;
                            //}
                        }

                    }

                    //}
                   // TotalBillAmount -= invoiceAmount;
                    Debug.WriteLine(TotalBillAmount);

                }
                List<ProductsDetailsInvoice> InvoiceList = new List<ProductsDetailsInvoice>();
                ProductsDetailsInvoice InvoiceListProduct = new ProductsDetailsInvoice();
                Debug.WriteLine(inv);
               
                //var distictInvIds = inv.Distinct().Select(x => x.InvoiceNumber).ToList();
                var distictInvIds = inv.GroupBy( i => new { i.InvoiceNumber, i.Id }).Select(g => new ProductsDetailsInvoice
                {
                    InvoiceNumber= g.Key.InvoiceNumber,
                    Id = g.Key.Id,
                    Name = g.First().Name,
                    TotalPrice = g.First().TotalPrice,
                    DateInvoice = g.First().DateInvoice,
                    Quantity = g.Sum(i => i.Quantity),
                    Price = g.Sum(i => i.Price),
                }).ToList();
                //var IDs = distictInvIds.GroupBy().toList();
                //return Json(inv);

                //var distictInvIdstotal =distictInvIds.Where( q=>q.InvoiceNumber)GroupBy(i => new { i.InvoiceNumber }).Select(g => new ProductsDetailsInvoice
                //{
                //    InvoiceNumber = g.Key.InvoiceNumber,
                //    Name = g.First().Name,
                //    DateInvoice = g.First().DateInvoice,
                //    Quantity = g.First().Quantity,
                //    Price = g.First().Price,
                //    TotalPrice = g.Sum(i => i.Price)
                //}).ToList();

                ViewBag.MyList = distictInvIds;
                //return new  ViewAsPdf("InvoiceList", distictInvIds);
                //TempData["list"] = distictInvIds;
                var test = JsonConvert.SerializeObject(distictInvIds);
                //return RedirectToAction("PrintInvoice", new { serializedModel =test });

                var url = this.Url.Action("PrintInvoice", "Invoice", new { serializedModel = test });

                return Json(new { status = "success", redirectUrl = url });
                // return PartialView("_InvoiceProcess");
            }
            catch (Exception ex)
            {

                throw;

            }
        }

        [RequestSizeLimit(40000000)]
        public IActionResult PrintInvoice(string serializedModel)
        {
            //ProductsDetailsInvoice p =new ProductsDetailsInvoice();
            //p.InvoiceNumber = item.id;
            //var model = TempData["list"] as List<ProductsDetailsInvoice>;
            List<ProductsDetailsInvoice> model = JsonConvert.DeserializeObject<List<ProductsDetailsInvoice>>(serializedModel);
            ViewBag.MyList = model;
            return new ViewAsPdf("InvoiceList", model);
            
        }

    }
}
