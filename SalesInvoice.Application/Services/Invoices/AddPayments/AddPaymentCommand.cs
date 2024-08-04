using MediatR;
using SalesInvoice.Application.Services.Invoices.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.Services.Invoices.AddPayments
{
   public sealed record AddPaymentCommand : IRequest<InvoiceResponseDto>
   {
      public decimal Amount { get; set; }
   }
}
