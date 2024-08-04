using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Domain
{
   public class AppConstants
   {
      public const string InvoiceStatusPending = "pending";
      public const string InvoiceStatusPaid = "paid";
      public const string InvoiceStatusVoid = "void";

      public const string DefaultDateFormat = "yyyy-MM-dd";
   }
}
