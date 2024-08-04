using Xunit;
using FluentValidation.TestHelper;
using SalesInvoice.Application.Services.Invoices.CreateInvoice;
using SalesInvoice.Domain;
using FluentAssertions;
using SalesInvoice.Domain.Entities;
using Moq;
using SalesInvoice.Application.Repositories;
using AutoMapper;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using SalesInvoice.Domain.Common;

namespace SalesInvoice.Application.UnitTests.Invoices
{
   public class CreateInvoicesTests
   {
      private readonly Mock<IUnitOfWork> _unitOfWork;
      private readonly Mock<IInvoiceRepository> _invoiceRepository;
      private readonly Mock<IMapper> _iMapper;
      private readonly CreateInvoiceHandler _handler;

      public CreateInvoicesTests()
      {
         _unitOfWork = new Mock<IUnitOfWork>();
         _invoiceRepository = new Mock<IInvoiceRepository>();
         _iMapper = new Mock<IMapper>();
         _handler = new CreateInvoiceHandler(_unitOfWork.Object, _invoiceRepository.Object, _iMapper.Object);
      }

      #region Invoice Validation Tests
      [Fact]
      public void CreateInvoiceValidator_IfValidAmountandValidDate_ShouldSuccess()
      {
         //arrange
         var validator = new CreateInvoiceValidator();

         var invoiceCommand = new CreateInvoiceCommand()
         {
            Amount = 100,
            Due_Date = DateTime.Now.ToString(AppConstants.DefaultDateFormat)
         };

         //act
         var result = validator.TestValidate(invoiceCommand);

         //assert
         result.IsValid.Should().BeTrue();
      }


      [Fact]
      public void CreateInvoiceValidator_IfInValidAmount_ShouldThrowValidationException()
      {
         //arrange
         var validator = new CreateInvoiceValidator();

         var invoiceCommand = new CreateInvoiceCommand()
         {
            Amount = -1,
            Due_Date = DateTime.Now.ToString(AppConstants.DefaultDateFormat)
         };


         //act
         var result = validator.TestValidate(invoiceCommand);

         //assert
         result.ShouldHaveValidationErrorFor(invo => invo.Amount);
      }

      #endregion


      #region Invoice Handler Tests
      [Fact]
      public async Task CreateInvoiceHandler_IfValidAmountandValidDate_ShouldReturnInvoice()
      {
         // arrange
         var invoiceCommand = new CreateInvoiceCommand()
         {
            Amount = 159.85M,
            Due_Date = DateTime.Now.ToString(AppConstants.DefaultDateFormat)
         };

         var invoice = Invoice.Create(invoiceCommand.Amount, invoiceCommand.Due_Date);
         var responseDto = new CreateInvoiceResponseDto { Id = "1234" };

         _iMapper.Setup(m => m.Map<CreateInvoiceResponseDto>(It.IsAny<Invoice>())).Returns(responseDto);

         // act
         var result = await _handler.Handle(invoiceCommand, default);

         // assert
         _invoiceRepository.Verify(repo => repo.Create(It.IsAny<Invoice>()), Times.Once);
         _unitOfWork.Verify(uow => uow.Save(It.IsAny<CancellationToken>()), Times.Once);
         _iMapper.Verify(m => m.Map<CreateInvoiceResponseDto>(It.IsAny<Invoice>()), Times.Once);

         Assert.NotNull(result);
         Assert.Equal(responseDto.Id, result.Id);
      }


      [Fact]
      public async Task CreateInvoiceHandler_IfInValidAmount_ShouldReturnFailure()
      {
         // arrange
         var request = new CreateInvoiceCommand
         {
            Amount = -50m, // Invalid amount
            Due_Date = DateTime.Now.ToString(AppConstants.DefaultDateFormat)
         };

         // act
         var exception = await Assert.ThrowsAsync<BusinessRuleValidationException>(() => _handler.Handle(request, default));

         // assert
         Assert.NotNull(exception);
         Assert.Contains(InvoiceErrors.InvalidAmount, exception.Errors);
      }


      [Fact]
      public async Task CreateInvoiceHandler_IfInValidDate_ShouldReturnFailure()
      {
         // arrange
         var request = new CreateInvoiceCommand
         {
            Amount = 145,
            Due_Date = "Invalid Date"
         };

         // act
         var exception = await Assert.ThrowsAsync<BusinessRuleValidationException>(() => _handler.Handle(request, default));

         // assert
         Assert.NotNull(exception);
         Assert.Contains(InvoiceErrors.InvalidDate, exception.Errors);
      }

      #endregion

   }
}
