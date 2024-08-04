using Microsoft.EntityFrameworkCore;
using SalesInvoice.Application.Repositories;
using SalesInvoice.Domain.Common;
using SalesInvoice.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Infrastructure.Repositories
{
   public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
   {
      protected readonly SalesInvoiceContext _context;

      public BaseRepository(SalesInvoiceContext context)
      {
         _context = context;
      }

      public void Create(T entity)
      {
         _context.Add(entity);
      }

      public void Update(T entity)
      {
         _context.Update(entity);
      }

      public void Delete(T entity)
      {
         entity.CreatedDate = DateTimeOffset.UtcNow;
         entity.IsDeleted = true;
         _context.Update(entity);
      }

      public Task<T?> Get(long id, CancellationToken cancellationToken)
      {
         return _context.Set<T>().FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id, cancellationToken);
      }

      public Task<List<T>> GetAll(CancellationToken cancellationToken)
      {
         return _context.Set<T>().Where(x => !x.IsDeleted).ToListAsync(cancellationToken);
      }
   }
}
