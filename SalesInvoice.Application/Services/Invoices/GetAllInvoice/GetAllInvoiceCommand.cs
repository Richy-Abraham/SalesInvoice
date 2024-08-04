using MediatR;
using SalesInvoice.Application.Services.Invoices.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.Services.Invoices.GetAllInvoice
{
   public sealed record GetAllInvoiceCommand : IRequest<List<InvoiceResponseDto>>;
}
