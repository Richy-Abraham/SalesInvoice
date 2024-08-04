using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Domain.Common
{
   public abstract class BaseEntity
   {
      public long Id { get; set; }
      public DateTimeOffset CreatedDate { get; set; }
      public DateTimeOffset? LastUpdatedDate { get; set; }
      public bool IsDeleted { get; set; }
      public DateTimeOffset? DeletedDate { get; set; }
   }
}
