using FluentValidation;
using SalesInvoice.Application.Services.Invoices.CreateInvoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.Services.Invoices.ProcessOverdue
{
   public sealed class ProcessOverdueValidator : AbstractValidator<ProcessOverdueCommand>
   {
      public ProcessOverdueValidator()
      {
         RuleFor(x => x.Overdue_Days).GreaterThan(0);
      }
   }
}
