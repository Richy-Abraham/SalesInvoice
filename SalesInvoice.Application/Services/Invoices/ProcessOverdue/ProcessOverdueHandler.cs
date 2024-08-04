using AutoMapper;
using MediatR;
using SalesInvoice.Application.Common.Exceptions;
using SalesInvoice.Application.Repositories;
using SalesInvoice.Application.Services.Invoices.AddPayments;
using SalesInvoice.Application.Services.Invoices.Common;
using SalesInvoice.Domain;
using SalesInvoice.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.Services.Invoices.ProcessOverdue
{
   public sealed class ProcessOverdueHandler : IRequestHandler<ProcessOverdueCommand, bool>
   {
      private readonly IUnitOfWork _unitOfWork;
      private readonly IInvoiceRepository _invoiceRepository;
      private readonly IMapper _mapper;

      public ProcessOverdueHandler(IUnitOfWork unitOfWork, IInvoiceRepository invoiceRepository, IMapper mapper)
      {
         _unitOfWork = unitOfWork;
         _invoiceRepository = invoiceRepository;
         _mapper = mapper;
      }

      public async Task<bool> Handle(ProcessOverdueCommand request, CancellationToken cancellationToken)
      {
         var now = DateTime.Now;
         var newInvoices = new List<Invoice>();
         var oldInvoices = _invoiceRepository.GetAll(cancellationToken).Result
                     .Where(i => i.Status == AppConstants.InvoiceStatusPending && i.Due_Date < now).ToList();
         foreach (var invoice in oldInvoices)
         {
            if (invoice.Paid_Amount > 0 && invoice.Paid_Amount < invoice.Amount)
            {
               // Partially paid, create a new invoice for remaining amount + late fee
               var remainingAmount = invoice.Amount - invoice.Paid_Amount;
               var newInvoice = Invoice.Create(remainingAmount + request.Late_Fee, now.AddDays(request.Overdue_Days).ToString());
               newInvoices.Add(newInvoice);
               invoice.Status = AppConstants.InvoiceStatusPaid; // Mark original as paid
            }
            else if (invoice.Paid_Amount == 0)
            {
               // Not paid at all, mark as void and create a new invoice
               var newInvoice = Invoice.Create(invoice.Amount + request.Late_Fee, now.AddDays(request.Overdue_Days).ToString());
               newInvoices.Add(newInvoice);
               invoice.Status = AppConstants.InvoiceStatusVoid; // Mark original as void
            }
         }

         await _invoiceRepository.AddRange(newInvoices, cancellationToken);
         await _invoiceRepository.UpdateRange(oldInvoices, cancellationToken);
         await _unitOfWork.Save(cancellationToken);

         return true;
      }
   }
}
