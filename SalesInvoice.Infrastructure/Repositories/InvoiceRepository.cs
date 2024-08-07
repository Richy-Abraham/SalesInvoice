﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SalesInvoice.Application.Repositories;
using SalesInvoice.Domain.Entities;
using SalesInvoice.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Infrastructure.Repositories
{
   public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
   {
      public InvoiceRepository(SalesInvoiceContext context) : base(context)
      {
      }

      public async Task<bool> AddRange(List<Invoice> invoices, CancellationToken cancellationToken)
      {
         await _context.Invoices.AddRangeAsync(invoices, cancellationToken);
         return true;
      }
      public async Task<bool> UpdateRange(List<Invoice> invoices, CancellationToken cancellationToken)
      {
         await Task.Run(() =>
         {
            _context.Invoices.UpdateRange(invoices);
         }, cancellationToken);
         return true;
      }
   }
}
