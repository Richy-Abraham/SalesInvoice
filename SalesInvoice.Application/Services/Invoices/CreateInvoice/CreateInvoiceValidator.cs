using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.Services.Invoices.CreateInvoice
{
   public sealed class CreateInvoiceValidator : AbstractValidator<CreateInvoiceCommand>
   {
      public CreateInvoiceValidator()
      {
         RuleFor(x => x.Amount).GreaterThan(0);
         //RuleFor(x => x.Due_Date.Year).GreaterThanOrEqualTo(DateTime.Now.Year).WithMessage("Due Date should be atleast of this year");
      }
   }
}
