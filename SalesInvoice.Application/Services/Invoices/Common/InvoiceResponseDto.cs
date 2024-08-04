using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.Services.Invoices.Common
{
   public sealed record InvoiceResponseDto
   {
      public string Id { get; set; } = string.Empty;
      public decimal Amount { get; set; }
      public decimal Paid_Amount { get; set; } = 0;
      public string Due_Date { get; set; } = string.Empty;
      public string Status { get; set; } = string.Empty;
   }
}
