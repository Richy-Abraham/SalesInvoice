using FluentValidation;
using SalesInvoice.Application.Services.Invoices.CreateInvoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.Services.Invoices.AddPayments
{
   public sealed class AddPaymentValidator : AbstractValidator<InvoicePaymentCommand>
   {
      public AddPaymentValidator()
      {
         RuleFor(x => x.Id).GreaterThan(0);
         RuleFor(x => x.Amount).GreaterThan(0);
      }
   }
}
