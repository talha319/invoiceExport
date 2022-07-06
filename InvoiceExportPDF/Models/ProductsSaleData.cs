using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceExportPDF.Models
{
    public class ProductsSaleData
    {

        public string ProductName { get; set; }
        public string Quantity { get; set; }
        public string ProductID { get; set; }
    }

    public class ProductsDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int RemainingQuantity { get; set; }
        public decimal Price { get; set; }
        public decimal Gsttex { get; set; }
    }

    public class ProductsDetailsInvoice
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Gsttex { get; set; }
        public long InvoiceNumber { get; set; }
        public DateTime DateInvoice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
