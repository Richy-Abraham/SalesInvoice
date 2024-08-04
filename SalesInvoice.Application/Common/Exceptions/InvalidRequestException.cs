using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.Common.Exceptions
{
   public class InvalidRequestException : Exception
   {
      public InvalidRequestException(string error) : base(error)
      {
         Errors = new List<string> { error };
      }

      public List<string> Errors { get; }

      public InvalidRequestException(List<string> errors)
      {
         Errors = errors;
      }
   }
}
