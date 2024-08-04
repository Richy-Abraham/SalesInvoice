using AutoMapper;
using SalesInvoice.Application.Services.Invoices.CreateInvoice;
using SalesInvoice.Domain;
using SalesInvoice.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.Services.Invoices.Common
{
   public sealed class InvoiceMapper : Profile
   {
      public InvoiceMapper()
      {
         CreateMap<CreateInvoiceCommand, Invoice>()
            .ForMember(dest => dest.Due_Date, opt => opt.MapFrom(src => src.Due_Date));
         CreateMap<Invoice, CreateInvoiceResponseDto>();
         CreateMap<Invoice, InvoiceResponseDto>()
            .ForMember(dest => dest.Due_Date, opt => opt.MapFrom(src => src.Due_Date.ToString(AppConstants.DefaultDateFormat)));
      }
   }
}
