using FluentAssertions;
using SalesInvoice.Domain.Common;
using SalesInvoice.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Domain.UnitTests.Invoices
{
   public class InvoiceTests
   { 
      [Fact]
      public void CreateInvoice_ValidDateAndValidAmountIsPassed_ShouldCreateInvoiceObject()
      {
         //arrange
         string validDate = "2023-08-03";
         decimal validAmount = 145.95M;

         //act
         var invoice = Invoice.Create(validAmount, validDate);

         //assert
         invoice.Should().NotBeNull();
      }

      [Fact]
      public void CreateInvoice_InValidDateAndInvalidAmountIsPassed_ShouldThrowException()
      {
         //arrange
         string invalidDate = "InvalidDate";
         decimal invalidAmount = -1;

         //act
         Action act = () => Invoice.Create(invalidAmount, invalidDate);

         //assert
         act.Should().Throw<BusinessRuleValidationException>()
                     .Where(ex => ex.Errors.First().Equals(InvoiceErrors.InvalidDate) ||
                                    ex.Errors.First().Equals(InvoiceErrors.InvalidDate));
      }

      [Fact]
      public void CreateInvoice_ValidAmountAndInvalidDateIsPassed_ShouldThrowException()
      {
         //arrange
         string invalidDate = "2023-2023-2023";
         decimal validAmount = 145.95M;

         //act
         Action act = () => Invoice.Create(validAmount, invalidDate);

         //assert
         act.Should().Throw<BusinessRuleValidationException>()
                     .Where(ex => ex.Errors.First().Equals(InvoiceErrors.InvalidDate));
      }

      [Fact]
      public void CreateInvoice_InValidAmountAndValidDateIsPassed_ShouldThrowException()
      {
         //arrange
         string validDate = "2023-08-03";
         decimal invalidAmount = -1;

         //act
         Action act = () => Invoice.Create(invalidAmount, validDate);

         //assert
         act.Should().Throw<BusinessRuleValidationException>()
                     .Where(ex => ex.Errors.First().Equals(InvoiceErrors.InvalidAmount));
      }
   }
}
