using AutoMapper;
using MediatR;
using SalesInvoice.Application.Repositories;
using SalesInvoice.Application.Services.Invoices.Common;
using SalesInvoice.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.Services.Invoices.CreateInvoice
{
   public sealed class CreateInvoiceHandler : IRequestHandler<CreateInvoiceCommand, CreateInvoiceResponseDto>
   {
      private readonly IUnitOfWork _unitOfWork;
      private readonly IInvoiceRepository _invoiceRepository;
      private readonly IMapper _mapper;

      public CreateInvoiceHandler(IUnitOfWork unitOfWork, IInvoiceRepository invoiceRepository, IMapper mapper)
      {
         _unitOfWork = unitOfWork;
         _invoiceRepository = invoiceRepository;
         _mapper = mapper;
      }

      public async Task<CreateInvoiceResponseDto> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
      {
         var invoice = Invoice.Create(request.Amount, request.Due_Date);
         _invoiceRepository.Create(invoice);
         await _unitOfWork.Save(cancellationToken);

         return _mapper.Map<CreateInvoiceResponseDto>(invoice);
      }
   }
}
