using Xunit;
using MiniBank.Core.Domains.Transactions.Validators;
using MiniBank.Core.Domains.Transactions;
using MiniBank.Core.Domains.Accounts.Repositories;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;
using Moq;
using System;

namespace MiniBank.Core.Tests.ValidatorTests
{
    public class TransactionValidatorTests
    {
        private readonly IValidator<Transaction> _transactionValidator;
        public TransactionValidatorTests()
        {
            _transactionValidator = new TransactionValidator();
        }
        [Fact]
        public async Task TransactionValidator_WithEmptyFromAccountId_ShouldThrowException()
        {
            // Arrange
            var user = new Transaction()
            {
                FromAccountId = Guid.Empty,
                ToAccountId = Guid.NewGuid(),
                Amount = 100
            };

            // Act
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _transactionValidator.ValidateAndThrowAsync(user));
            Assert.Equal("Validation failed: \r\n -- FromAccountId: Не должен быть пустым Severity: Error", exception.Message);
        }
        [Fact]
        public async Task TransactionValidator_WithEmptyToAccountId_ShouldThrowException()
        {
            // Arrange
            var user = new Transaction()
            {
                FromAccountId = Guid.NewGuid(),
                ToAccountId = Guid.Empty,
                Amount = 100
            };

            // Act
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _transactionValidator.ValidateAndThrowAsync(user));

            // Assert
            Assert.Equal("Validation failed: \r\n -- ToAccountId: Не должен быть пустым Severity: Error", exception.Message);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10.05)]
        [InlineData(-100)]
        [InlineData(-1550.55)]
        public async Task TransactionValidator_WithAmountLessOrEqualZero_ShouldThrowException(decimal amount)
        {
            // Arrange
            var user = new Transaction()
            {
                FromAccountId = Guid.NewGuid(),
                ToAccountId = Guid.NewGuid(),
                Amount = amount
            };

            // Act
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _transactionValidator.ValidateAndThrowAsync(user));
            
            //Assert
            Assert.Equal("Validation failed: \r\n -- Amount: Должен быть больше нуля Severity: Error", exception.Message);
        }
        [Fact]
        public async Task TransactionValidator_WithEqualAccountsId_ShouldThrowException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var user = new Transaction()
            {
                FromAccountId = id,
                ToAccountId = id,
                Amount = 100
            };

            // Act
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _transactionValidator.ValidateAndThrowAsync(user));
            
            // Assert
            Assert.Equal("Validation failed: \r\n -- AccountsId: Не должны совпадать Severity: Error", exception.Message);
        }
        [Theory]
        [InlineData(0.89)]
        [InlineData(50)]
        [InlineData(121.5)]
        [InlineData(5347.55)]
        public async Task TransactionValidator_WithCorrectParameters_ShouldNotThrowException(decimal amount)
        {
            // Arrange
            var transaction = new Transaction()
            {
                FromAccountId= Guid.NewGuid(),
                ToAccountId= Guid.NewGuid(),
                Amount= amount
            };

            // Act
            var exception = await Record.ExceptionAsync(() => _transactionValidator.ValidateAndThrowAsync(transaction));
            
            //Assert
            Assert.Null(exception);
        }
    }
}
