using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SalesInvoice.Application.Common.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application
{
   public static class ServiceExtensions
   {
      public static IServiceCollection AddApplication(this IServiceCollection services)
      {
         var assembly = typeof(ServiceExtensions).Assembly;
         services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));
         services.AddValidatorsFromAssembly(assembly);
         services.AddAutoMapper(assembly);
         services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
         return services;
      }
   }
}
