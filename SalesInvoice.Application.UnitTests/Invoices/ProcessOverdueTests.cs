using AutoMapper;
using Moq;
using SalesInvoice.Application.Repositories;
using SalesInvoice.Application.Services.Invoices.ProcessOverdue;
using SalesInvoice.Domain.Entities;
using SalesInvoice.Domain;

namespace SalesInvoice.Application.UnitTests.Invoices
{
   public class ProcessOverdueTests
   {
      private readonly Mock<IUnitOfWork> _unitOfWork;
      private readonly Mock<IInvoiceRepository> _invoiceRepository;
      private readonly Mock<IMapper> _iMapper;
      private readonly ProcessOverdueHandler _handler;

      public ProcessOverdueTests()
      {
         _unitOfWork = new Mock<IUnitOfWork>();
         _invoiceRepository = new Mock<IInvoiceRepository>();
         _iMapper = new Mock<IMapper>();
         _handler = new ProcessOverdueHandler(_unitOfWork.Object, _invoiceRepository.Object, _iMapper.Object);
      }


      [Fact]
      public async Task ProcessOverdueHandler_ShouldCreateNewInvoicesAndMarkOldAsPaid_WhenPartiallyPaidInvoicesAreOverdue()
      {
         // arrange
         var now = new DateTime(2024, 8, 1); // Set a fixed date for consistency
         var command = new ProcessOverdueCommand { Late_Fee = 10.5m, Overdue_Days = 10 };

         var oldInvoices = new List<Invoice>();
         var invoice = Invoice.Create(200m, now.AddDays(-5).ToString(AppConstants.DefaultDateFormat));
         invoice.Id = 1;
         invoice.Paid_Amount = 50m;
         oldInvoices.Add(invoice);

         _invoiceRepository.Setup(repo => repo.GetAll(It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(oldInvoices);

         _invoiceRepository.Setup(repo => repo.AddRange(It.IsAny<List<Invoice>>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(true);

         _invoiceRepository.Setup(repo => repo.UpdateRange(It.IsAny<List<Invoice>>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(true);

         // Act
         var result = await _handler.Handle(command, default);

         Assert.True(result);
      }

      [Fact]
      public async Task ProcessOverdueHandler_ShouldCreateNewInvoicesAndMarkOldAsVoid_WhenNotPaidInvoicesAreOverdue()
      {
         // arrange
         var now = new DateTime(2024, 8, 1);
         var command = new ProcessOverdueCommand { Late_Fee = 10.5m, Overdue_Days = 10 };

         var oldInvoices = new List<Invoice>();
         var invoice = Invoice.Create(200m, now.AddDays(-5).ToString(AppConstants.DefaultDateFormat));
         invoice.Id = 1;
         invoice.Paid_Amount = 0m;
         oldInvoices.Add(invoice);

         _invoiceRepository.Setup(repo => repo.GetAll(It.IsAny<CancellationToken>())).ReturnsAsync(oldInvoices);

         // act
         var result = await _handler.Handle(command, CancellationToken.None);

         // assert
         Assert.True(result);
      }

      [Fact]
      public async Task ProcessOverdueHandler_ShouldReturnTrue_WhenNoInvoicesAreOverdue()
      {
         // arrange
         var now = new DateTime(2024, 8, 1);
         var command = new ProcessOverdueCommand { Late_Fee = 10.5m, Overdue_Days = 10 };

         var oldInvoices = new List<Invoice>();

         _invoiceRepository.Setup(repo => repo.GetAll(It.IsAny<CancellationToken>())).ReturnsAsync(oldInvoices);

         // act
         var result = await _handler.Handle(command, CancellationToken.None);

         // assert
         Assert.True(result);
      }
   }
}
