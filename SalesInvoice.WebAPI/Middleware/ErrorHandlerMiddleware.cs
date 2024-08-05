using Microsoft.AspNetCore.Mvc;
using SalesInvoice.Application.Common.Exceptions;
using SalesInvoice.Domain.Common;
using SalesInvoice.WebAPI.CustomProblemDetails;
using System.Net;
using System.Text.Json;

namespace SalesInvoice.WebAPI.Middleware
{
   public class ErrorHandlerMiddleware
   {
      private readonly RequestDelegate _next;
      private readonly IWebHostEnvironment _env;

      private readonly ILogger<ErrorHandlerMiddleware> _logger;

      public ErrorHandlerMiddleware(RequestDelegate next, IWebHostEnvironment env, ILogger<ErrorHandlerMiddleware> logger)
      {
         _next = next;
         _env = env;
         _logger = logger;
      }

      public async Task Invoke(HttpContext context)
      {
         try
         {
            await _next(context);
         }
         catch (InvalidRequestException ex)
         {
            _logger.LogError(ex, ex.Message);
            var problemDetails = GetBadRequestProblemDetails(ex);

            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            await response.WriteAsync(JsonSerializer.Serialize(problemDetails));
         }
         catch (BusinessRuleValidationException ex)
         {
            _logger.LogError(ex, ex.Message);
            var problemDetails = GetBusinessRuleValidationProblemDetails(ex);

            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            await response.WriteAsync(JsonSerializer.Serialize(problemDetails));
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, ex.Message);
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            ProblemDetails problemDetails = GetProblemDetails(ex);

            await response.WriteAsync(JsonSerializer.Serialize(problemDetails));
         }
      }

      private ProblemDetails GetProblemDetails(Exception ex)
      {
         string traceId = Guid.NewGuid().ToString();

         if (_env.EnvironmentName == "Development")
         {
            return new ProblemDetails
            {
               Status = (int)HttpStatusCode.InternalServerError,
               Type = "https://httpstatuses.com/500",
               Title = ex.Message,
               Detail = ex.ToString(),
               Instance = traceId
            };
         }
         else
         {
            return new ProblemDetails
            {
               Status = (int)HttpStatusCode.InternalServerError,
               Type = "https://httpstatuses.com/500",
               Title = "Something went wrong. Please try after some time",
               Detail = @"We apologize for inconvenience. Please let us know about the error at support@orion.com. Include traceId: {traceId} in email",
               Instance = traceId
            };
         }
      }

      private InvalidRequestProblemDetails GetBadRequestProblemDetails(InvalidRequestException ex)
      {
         string traceId = Guid.NewGuid().ToString();

         var invalidRequestProblemDetails = new InvalidRequestProblemDetails(ex, traceId);

         return invalidRequestProblemDetails;
      }

      private BusinessRuleValidationProblemDetails GetBusinessRuleValidationProblemDetails(BusinessRuleValidationException ex)
      {
         string traceId = Guid.NewGuid().ToString();

         var businessRuleValidationProblemDetails = new BusinessRuleValidationProblemDetails(ex, traceId);

         return businessRuleValidationProblemDetails;
      }

   }
}
