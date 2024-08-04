using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Domain.Common
{
   public class BusinessRuleValidationException : Exception
   {
      public List<string> Errors { get; }

      public BusinessRuleValidationException(List<string> errors)
      {
         Errors = errors;
      }

      public BusinessRuleValidationException(string error) : base(error)
      {
         Errors = new List<string> { error };
      }
   }
}
