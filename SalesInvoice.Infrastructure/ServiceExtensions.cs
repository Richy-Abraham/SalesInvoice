using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesInvoice.Application.Repositories;
using SalesInvoice.Infrastructure.Context;
using SalesInvoice.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Infrastructure
{
   public static class ServiceExtensions
   {
      public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
      {
         var connectionString = configuration.GetConnectionString("DefaultSqlite");
         services.AddDbContext<SalesInvoiceContext>(opt => opt.UseSqlite(connectionString));

         services.AddScoped<IUnitOfWork, UnitOfWork>();
         services.AddScoped<IInvoiceRepository, InvoiceRepository>();
         return services;
      }
   }
}
