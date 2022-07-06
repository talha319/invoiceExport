using System;
using System.Collections.Generic;

#nullable disable

namespace InvoiceExportPDF.Data
{
    public partial class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal? Qty { get; set; }
        public string Uom { get; set; }
        public decimal? Price { get; set; }
        public decimal? Gsttex { get; set; }
        public decimal? OtherTex { get; set; }
    }
    public class ProductModel
    {
        public List<Product> PrductList { get; set; }
    }
}
