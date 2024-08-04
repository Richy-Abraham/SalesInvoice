using MediatR;
using SalesInvoice.Application.Services.Invoices.Common;
using SalesInvoice.Domain;

namespace SalesInvoice.Application.Services.Invoices.CreateInvoice
{
   public sealed record CreateInvoiceCommand : IRequest<CreateInvoiceResponseDto>
   {
      public decimal Amount { get; set; }
      public string Due_Date { get; set; } = DateTime.Now.ToString(AppConstants.DefaultDateFormat);
   }
}
