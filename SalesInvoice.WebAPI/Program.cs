using SalesInvoice.Infrastructure;
using SalesInvoice.Application;
using Serilog;
using Microsoft.EntityFrameworkCore;
using SalesInvoice.Infrastructure.Context;
using SalesInvoice.WebAPI.Extensions;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SalesInvoiceContext>(options =>
         options.UseSqlite(
         builder.Configuration.GetConnectionString("DefaultSqlite")));
builder.Services.ConfigureApiBehavior();
builder.Services.ConfigureCorsPolicy();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication()
               .AddInfrastructure(builder.Configuration);

// Add serilog services to the container and read config from appsettings
builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthorization();

// Configure the HTTP request pipeline.
app.UseMiddleware<ErrorHandlerMiddleware>(app.Environment);
app.UseCors();
app.MapControllers();

app.Run();
