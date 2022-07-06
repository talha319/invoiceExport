using System;
using System.Collections.Generic;

#nullable disable

namespace InvoiceExportPDF.Data
{
    public partial class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }
}
