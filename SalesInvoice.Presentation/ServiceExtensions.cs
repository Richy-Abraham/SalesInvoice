﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Presentation
{
   public static class ServiceExtensions
   {
      public static IServiceCollection AddPresentation(this IServiceCollection services)
      {
         return services;
      }
   }
}
