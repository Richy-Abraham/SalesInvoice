using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Domain.Entities
{
   public static class InvoiceErrors
   {
      public const string InvoiceNotFound = "Invoice Not Found";
      public const string InvalidDate = "Invalid Date";
      public const string InvalidAmount = "Invalid Amount";
   }
}
