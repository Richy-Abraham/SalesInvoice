using Microsoft.AspNetCore.Mvc;
using SalesInvoice.Application.Common.Exceptions;

namespace SalesInvoice.WebAPI.CustomProblemDetails
{
   public class InvalidRequestProblemDetails : ProblemDetails
   {
      public List<string> Errors { get; }

      public InvalidRequestProblemDetails(InvalidRequestException exception, string traceId)
      {
         Title = "Request validation error";
         Status = StatusCodes.Status400BadRequest;
         Type = "https://httpstatuses.com/400";
         Errors = exception.Errors;
         Instance = traceId;
      }
   }
}
