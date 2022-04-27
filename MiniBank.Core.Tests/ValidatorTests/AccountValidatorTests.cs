using Xunit;
using MiniBank.Core.Domains.Accounts.Validators;
using MiniBank.Core.Domains.Accounts;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;
using System;

namespace MiniBank.Core.Tests.ValidatorTests
{
    public class AccountValidatorTests
    {
        private readonly IValidator<Account> _accountValidator;
        public AccountValidatorTests()
        {
            _accountValidator = new AccountValidator();
        }
        [Fact]
        public async Task AccountValidator_WithEmptyUserId_ShouldThrowException()
        {
            // Arrange
            var user = new Account()
            {
                UserId = Guid.Empty,
                Balance = 0,
                CurrencyName = "USD"
            };

            // Act
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _accountValidator.ValidateAndThrowAsync(user));

            // Assert
            Assert.Equal("Validation failed: \r\n -- UserId: Не должен быть пустым Severity: Error", exception.Message);
        }
        [Theory]
        [InlineData(-1000.52)]
        [InlineData(-100)]
        [InlineData(-50.5)]
        [InlineData(-1)]
        [InlineData(-0.1)]
        public async Task AccountValidator_WithNegativeBalance_ShouldThrowException(decimal balance)
        {
            // Arrange
            var user = new Account()
            {
                UserId = Guid.NewGuid(),
                Balance = balance,
                CurrencyName = "USD"
            };

            // Act
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _accountValidator.ValidateAndThrowAsync(user));
            
            // Assert
            Assert.Equal("Validation failed: \r\n -- Balance: Должен быть не меньше нуля Severity: Error", exception.Message);
        }
        [Theory]
        [InlineData("")]
        [InlineData("OK")]
        [InlineData("RUBLE")]
        [InlineData("DOLLAR")]
        public async Task AccountValidator_WithCurrencyNameLengthNotEqualToThree_ShouldThrowException(string currencyName)
        {
            // Arrange
            var user = new Account()
            {
                UserId = Guid.NewGuid(),
                Balance = 0,
                CurrencyName = currencyName
            };

            // Act
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _accountValidator.ValidateAndThrowAsync(user));
            
            //Assert
            Assert.Equal("Validation failed: \r\n -- CurrencyName.Length: Должна быть равна 3 Severity: Error", exception.Message);
        }
        [Theory]
        [InlineData("usd")]
        [InlineData("Usd")]
        [InlineData("USd")]
        [InlineData("usD")]
        [InlineData("UsD")]
        public async Task AccountValidator_WithCurrencyNameNotInUpperCase_ShouldThrowException(string currencyName)
        {
            // Arrange
            var user = new Account()
            {
                UserId = Guid.NewGuid(),
                Balance = 0,
                CurrencyName = currencyName
            };

            // Act
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _accountValidator.ValidateAndThrowAsync(user));

            // Assert
            Assert.Equal("Validation failed: \r\n -- CurrencyName: Должно состоять только из заглавных букв Severity: Error", exception.Message);
        }
        [Theory]
        [InlineData("EUR", 0)]
        [InlineData("USD", 50)]
        [InlineData("RUB", 121.5)]
        [InlineData("AZN", 5347.55)]
        public async Task AccountValidator_WithCorrectParameters_ShouldNotThrowException(string currencyName, decimal balance)
        {
            // Arrange
            var account = new Account()
            {
                UserId = Guid.NewGuid(),
                CurrencyName = currencyName,
                Balance = balance
            };

            // Act
            var exception = await Record.ExceptionAsync(() => _accountValidator.ValidateAndThrowAsync(account));

            // Assert
            Assert.Null(exception);
        }
    }
}
