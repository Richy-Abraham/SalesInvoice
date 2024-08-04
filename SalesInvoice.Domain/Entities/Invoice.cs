using SalesInvoice.Domain.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SalesInvoice.Domain.Entities
{
   public sealed class Invoice : BaseEntity
   {
      public decimal Amount { get; set; }
      public decimal Paid_Amount { get; set; } = 0;
      public DateTime Due_Date { get; set; }
      public string Status { get; set; } = AppConstants.InvoiceStatusPending;

      private Invoice()
      {
      }

      public static Invoice Create(decimal amount, string dueDate)
      {
         var invo = new Invoice();
         DateTime parsedDate;
         invo.Due_Date = !DateTime.TryParse(dueDate, out parsedDate) ? throw new BusinessRuleValidationException(InvoiceErrors.InvalidDate) : parsedDate;
         invo.Amount = amount <= 0 ? throw new BusinessRuleValidationException(InvoiceErrors.InvalidAmount) : amount;

         return invo;
      }

   }
}
