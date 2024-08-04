using AutoMapper;
using MediatR;
using SalesInvoice.Application.Repositories;
using SalesInvoice.Application.Services.Invoices.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.Services.Invoices.GetAllInvoice
{
   public sealed class GetAllInvoiceHandler : IRequestHandler<GetAllInvoiceCommand, List<InvoiceResponseDto>>
   {
      private readonly IInvoiceRepository _invoiceRepository;
      private readonly IMapper _mapper;

      public GetAllInvoiceHandler(IInvoiceRepository invoiceRepository, IMapper mapper)
      {
         _invoiceRepository = invoiceRepository;
         _mapper = mapper;
      }

      public async Task<List<InvoiceResponseDto>> Handle(GetAllInvoiceCommand request, CancellationToken cancellationToken)
      {
         var invoices = await _invoiceRepository.GetAll(cancellationToken);
         return _mapper.Map<List<InvoiceResponseDto>>(invoices);
      }
   }
}
