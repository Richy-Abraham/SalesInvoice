using AutoMapper;
using MediatR;
using SalesInvoice.Application.Common.Exceptions;
using SalesInvoice.Application.Repositories;
using SalesInvoice.Application.Services.Invoices.Common;
using SalesInvoice.Application.Services.Invoices.CreateInvoice;
using SalesInvoice.Domain;
using SalesInvoice.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SalesInvoice.Application.Services.Invoices.AddPayments
{
   public sealed class AddPaymentsHandler : IRequestHandler<InvoicePaymentCommand, InvoiceResponseDto>
   {
      private readonly IUnitOfWork _unitOfWork;
      private readonly IInvoiceRepository _invoiceRepository;
      private readonly IMapper _mapper;

      public AddPaymentsHandler(IUnitOfWork unitOfWork, IInvoiceRepository invoiceRepository, IMapper mapper)
      {
         _unitOfWork = unitOfWork;
         _invoiceRepository = invoiceRepository;
         _mapper = mapper;
      }

      public async Task<InvoiceResponseDto> Handle(InvoicePaymentCommand request, CancellationToken cancellationToken)
      {
         var invoice = await _invoiceRepository.Get(request.Id, cancellationToken);
         if (invoice == null || invoice.Status == AppConstants.InvoiceStatusVoid)
         {
            throw new NotFoundException(InvoiceErrors.InvoiceNotFound);
         }

         invoice.Paid_Amount += request.Amount;
         if (invoice.Paid_Amount >= invoice.Amount)
         {
            invoice.Status = AppConstants.InvoiceStatusPaid;
         } else
         {
            invoice.Status = AppConstants.InvoiceStatusPending;
         }
         _invoiceRepository.Update(invoice);
         await _unitOfWork.Save(cancellationToken);

         return _mapper.Map<InvoiceResponseDto>(invoice);
      }
   }
}
