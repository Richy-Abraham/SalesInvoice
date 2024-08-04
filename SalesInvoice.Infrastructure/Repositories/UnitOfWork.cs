using SalesInvoice.Application.Repositories;
using SalesInvoice.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Infrastructure.Repositories
{
   public class UnitOfWork : IUnitOfWork
   {
      private readonly SalesInvoiceContext _context;

      public UnitOfWork(SalesInvoiceContext context)
      {
         _context = context;
      }
      public Task Save(CancellationToken cancellationToken)
      {
         return _context.SaveChangesAsync(cancellationToken);
      }
   }
}
