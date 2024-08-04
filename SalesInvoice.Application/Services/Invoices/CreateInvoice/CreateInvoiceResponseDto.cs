using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.Services.Invoices.CreateInvoice
{
   public sealed record CreateInvoiceResponseDto
   {
      public long Id { get; set; }
   }
}
