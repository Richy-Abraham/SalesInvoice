using MediatR;
using Microsoft.AspNetCore.Mvc;
using SalesInvoice.Application.Services.Invoices.AddPayments;
using SalesInvoice.Application.Services.Invoices.Common;
using SalesInvoice.Application.Services.Invoices.CreateInvoice;
using SalesInvoice.Application.Services.Invoices.GetAllInvoice;
using SalesInvoice.Application.Services.Invoices.ProcessOverdue;
using System.Threading;

namespace SalesInvoice.WebAPI.Controllers
{
   [ApiController]
   [Route("[controller]")]
   public class InvoicesController : ControllerBase
   {
      private readonly IMediator _mediator;

      public InvoicesController(IMediator mediator)
      {
         _mediator = mediator;
      }

      [HttpGet]
      public async Task<ActionResult<List<InvoiceResponseDto>>> GetAll(CancellationToken cancellationToken)
      {
         var response = await _mediator.Send(new GetAllInvoiceCommand(), cancellationToken);
         return Ok(response);
      }

      [HttpPost]
      public async Task<ActionResult<CreateInvoiceResponseDto>> Create(CreateInvoiceCommand request,
          CancellationToken cancellationToken)
      {
         var response = await _mediator.Send(request, cancellationToken);
         return CreatedAtAction(nameof(Create), new { id = response.Id }, response);
      }

      [HttpPost("{id}/payments")]
      public async Task<ActionResult> PostPayment(long id, AddPaymentCommand request,
         CancellationToken cancellationToken)
      {
         var wrapReq = new InvoicePaymentCommand { Id = id, Amount = request?.Amount ?? 0 };
         var response = await _mediator.Send(wrapReq, cancellationToken);
         return Ok();
      }

      [HttpPost("process-overdue")]
      public async Task<ActionResult> ProcessOverdue(ProcessOverdueCommand request,
         CancellationToken cancellationToken)
      {
         var response = await _mediator.Send(request, cancellationToken);
         return Ok();
      }
   }
}
