using MediatR;
using SalesInvoice.Application.Services.Invoices.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.Services.Invoices.ProcessOverdue
{
   public sealed record ProcessOverdueCommand : IRequest<bool>
   {
      public decimal Late_Fee { get; set; }
      public int Overdue_Days { get; set; }
   }
}
