using SalesInvoice.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.Repositories
{
   public interface IInvoiceRepository : IBaseRepository<Invoice>
   {
      Task<bool> AddRange(List<Invoice> invoices, CancellationToken cancellationToken);
      Task <bool> UpdateRange(List<Invoice> invoices, CancellationToken cancellationToken);
   }
}
