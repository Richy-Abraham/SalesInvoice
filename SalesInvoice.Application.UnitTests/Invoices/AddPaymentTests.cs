using AutoMapper;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using SalesInvoice.Application.Repositories;
using SalesInvoice.Application.Services.Invoices.AddPayments;
using SalesInvoice.Application.Services.Invoices.CreateInvoice;
using SalesInvoice.Domain.Common;
using SalesInvoice.Domain.Entities;
using SalesInvoice.Domain;
using SalesInvoice.Application.Common.Exceptions;
using SalesInvoice.Application.Services.Invoices.Common;

namespace SalesInvoice.Application.UnitTests.Invoices
{
   public class AddPaymentTests
   {
      private readonly Mock<IUnitOfWork> _unitOfWork;
      private readonly Mock<IInvoiceRepository> _invoiceRepository;
      private readonly Mock<IMapper> _iMapper;
      private readonly AddPaymentsHandler _handler;

      public AddPaymentTests()
      {
         _unitOfWork = new Mock<IUnitOfWork>();
         _invoiceRepository = new Mock<IInvoiceRepository>();
         _iMapper = new Mock<IMapper>();
         _handler = new AddPaymentsHandler(_unitOfWork.Object, _invoiceRepository.Object, _iMapper.Object);
      }

      #region AddPayment Validator Tests
      [Fact]
      public void AddPaymentValidator_IfValidAmountAndValidId_ShouldSuccess()
      {
         //arrange
         var validator = new AddPaymentValidator();

         var paymentCommand = new InvoicePaymentCommand()
         {
            Id = 1,
            Amount = 100
         };

         //act
         var result = validator.TestValidate(paymentCommand);

         //assert
         result.IsValid.Should().BeTrue();
      }

      [Fact]
      public void AddPaymentValidator_IfInValidAmount_ShouldThrowValidationException()
      {
         //arrange
         var validator = new AddPaymentValidator();

         var paymentCommand = new InvoicePaymentCommand()
         {
            Id = 1,
            Amount = -1
         };

         //act
         var result = validator.TestValidate(paymentCommand);

         //assert
         result.ShouldHaveValidationErrorFor(pay => pay.Amount);
      }

      [Fact]
      public void AddPaymentValidator_IfInValidId_ShouldThrowValidationException()
      {
         //arrange
         var validator = new AddPaymentValidator();

         var paymentCommand = new InvoicePaymentCommand()
         {
            Id = 0,
            Amount = 100
         };

         //act
         var result = validator.TestValidate(paymentCommand);

         //assert
         result.ShouldHaveValidationErrorFor(pay => pay.Id);
      }

      #endregion


      #region AddPayment Handler Tests

      [Fact]
      public async Task AddPaymentsHandler_ShouldThrowNotFoundException_WhenInvoiceNotFoundOrVoid()
      {
         // arrange
         var paymentCommand = new InvoicePaymentCommand()
         {
            Id = 1,
            Amount = 159.85M
         };
         _invoiceRepository.Setup(repo => repo.Get(paymentCommand.Id, It.IsAny<CancellationToken>()))
                               .ReturnsAsync((Invoice?)null);

         // act
         var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(paymentCommand, default));

         // assert
         Assert.Equal(InvoiceErrors.InvoiceNotFound, exception.Message);
      }

      [Fact]
      public async Task AddPaymentsHandler_ShouldUpdateInvoiceStatusToPaid_WhenFullyPaid()
      {
         // arrange
         var paymentCommand = new InvoicePaymentCommand()
         {
            Id = 1234,
            Amount = 100.00m
         };
         var invoice = Invoice.Create(paymentCommand.Amount, DateTime.Now.ToString(AppConstants.DefaultDateFormat));
         invoice.Id = paymentCommand.Id;
         //invoice.Paid_Amount = 0.0m;
         //invoice.Status = AppConstants.InvoiceStatusPending;

         _invoiceRepository.Setup(repo => repo.Get(paymentCommand.Id, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(invoice);

         var responseDto = new InvoiceResponseDto { Id = 1234, Amount = 100.0m, Paid_Amount = 100.0m, Status = AppConstants.InvoiceStatusPaid };
         _iMapper.Setup(m => m.Map<InvoiceResponseDto>(It.IsAny<Invoice>())).Returns(responseDto);

         // act
         var result = await _handler.Handle(paymentCommand, default);

         // assert
         _invoiceRepository.Verify(repo => repo.Update(It.IsAny<Invoice>()), Times.Once);
         _unitOfWork.Verify(uow => uow.Save(It.IsAny<CancellationToken>()), Times.Once);
         _iMapper.Verify(m => m.Map<InvoiceResponseDto>(It.IsAny<Invoice>()), Times.Once);

         Assert.Equal(AppConstants.InvoiceStatusPaid, invoice.Status);
         Assert.Equal(responseDto.Paid_Amount, invoice.Paid_Amount);
         Assert.NotNull(result);
         Assert.Equal(responseDto, result);
      }

      [Fact]
      public async Task AddPaymentsHandler_ShouldUpdateInvoiceStatusToPending_WhenPartiallyPaid()
      {
         // arrange
         var paymentCommand = new InvoicePaymentCommand()
         {
            Id = 1234,
            Amount = 70.00m
         };
         var invoice = Invoice.Create(100.00m, DateTime.Now.ToString(AppConstants.DefaultDateFormat));
         invoice.Id = paymentCommand.Id;

         _invoiceRepository.Setup(repo => repo.Get(paymentCommand.Id, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(invoice);

         var responseDto = new InvoiceResponseDto { Id = 1234, Amount = 100.0m, Paid_Amount = 70.0m, Status = AppConstants.InvoiceStatusPending };
         _iMapper.Setup(m => m.Map<InvoiceResponseDto>(It.IsAny<Invoice>())).Returns(responseDto);

         // Act
         var result = await _handler.Handle(paymentCommand, default);

         // Assert
         _invoiceRepository.Verify(repo => repo.Update(It.IsAny<Invoice>()), Times.Once);
         _unitOfWork.Verify(uow => uow.Save(It.IsAny<CancellationToken>()), Times.Once);
         _iMapper.Verify(m => m.Map<InvoiceResponseDto>(It.IsAny<Invoice>()), Times.Once);

         Assert.Equal(AppConstants.InvoiceStatusPending, invoice.Status);
         Assert.Equal(responseDto.Paid_Amount, invoice.Paid_Amount);
         Assert.NotNull(result);
         Assert.Equal(responseDto, result);
      }

      #endregion
   }
}
