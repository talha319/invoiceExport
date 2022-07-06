using InvoiceExportPDF.Data;
using InvoiceExportPDF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceExportPDF.Service
{
    public class Helper
    {
        private readonly InvoiceExportDBContext _context;

        public Helper(InvoiceExportDBContext context)
        {
            _context = context;
        }

        // GET: Products
     

    }
}


