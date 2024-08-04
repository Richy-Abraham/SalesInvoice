using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.Services.Invoices.CreateInvoice
{
   public sealed record CreateInvoiceResponseDto
   {
      public string Id { get; set; } = string.Empty;
   }
}
