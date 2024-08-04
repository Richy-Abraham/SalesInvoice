using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SalesInvoice.Domain.Entities;
using SalesInvoice.Infrastructure.Repositories;

namespace SalesInvoice.Infrastructure.Context
{
   public class SalesInvoiceContext : DbContext
   {
      public SalesInvoiceContext(DbContextOptions<SalesInvoiceContext> options) : base(options)
      {
      }

      //static LoggerFactory object
      public static readonly ILoggerFactory loggerFactory = new LoggerFactory();

      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
         optionsBuilder.UseLoggerFactory(loggerFactory)  //tie-up DbContext with LoggerFactory object
             .EnableSensitiveDataLogging()
             .UseSqlite("Data Source=invoices.db")
             .LogTo(Console.WriteLine).EnableDetailedErrors();
         base.OnConfiguring(optionsBuilder);
      }

      public DbSet<Invoice> Invoices { get; set; }
   }
}
