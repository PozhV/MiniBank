using Xunit;
using MiniBank.Core.Domains.Transactions;
using MiniBank.Core.Domains.Transactions.Services;
using MiniBank.Core.Domains.Accounts.Repositories;
using Moq;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;
using System;
using System.Collections.Generic;
using MiniBank.Core.Domains.Users.UserMessages;

namespace MiniBank.Core.Tests.ServiceTests
{
    public class TransactionServiceTests
    {
        private readonly Mock<ITransactionRepository> _mockTransactionRepository;
        private readonly Mock<IValidator<Transaction>> _mockTransactionValidator;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Mock<IRatesDatabase> _mockRatesDatabase;
        private readonly Mock<IConverter> _mockConverter;
        private readonly ITransactionService _transactionService;
        public TransactionServiceTests()
        {
            _mockTransactionValidator = new Mock<IValidator<Transaction>>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockTransactionRepository = new Mock<ITransactionRepository>();
            _mockConverter = new Mock<IConverter>();
            _mockRatesDatabase = new Mock<IRatesDatabase>();
            _mockAccountRepository = new Mock<IAccountRepository>();
            _transactionService = new TransactionService(_mockTransactionValidator.Object, _mockUnitOfWork.Object, _mockTransactionRepository.Object,
                _mockAccountRepository.Object, _mockRatesDatabase.Object, _mockConverter.Object);
        }
        [Fact]
        public async Task ExecuteTransaction_WithAmountLessThanOrEqualToZero_ShouldThrowException_ShouldntCallsAnything()
        {
            // Arrange
            var validator = new InlineValidator<Transaction>();
            validator.RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Должен быть больше нуля");
            var transactionService = new TransactionService(validator, _mockUnitOfWork.Object, _mockTransactionRepository.Object,
                _mockAccountRepository.Object, _mockRatesDatabase.Object, _mockConverter.Object);
            var transaction = new Transaction()
            {
                Amount = -10
            };

            // Act
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => transactionService.ExecuteTransaction(transaction));

            // Arrange
            _mockAccountRepository.VerifyNoOtherCalls();
            _mockTransactionRepository.VerifyNoOtherCalls();
            _mockConverter.VerifyNoOtherCalls();
            _mockUnitOfWork.VerifyNoOtherCalls();
            _mockRatesDatabase.VerifyNoOtherCalls();
            Assert.Equal("Validation failed: \r\n -- Amount: Должен быть больше нуля Severity: Error", exception.Message);
        }
        [Fact]
        public async Task ExecuteTransaction_AccountDoesntExist_ShouldThrowException_ShouldntCallsConverter()
        {
            // Arrange
            var transaction = new Transaction()
            {
                FromAccountId = Guid.NewGuid(),
                ToAccountId = Guid.NewGuid(),
                Amount = 10
            };

            // Act
            _mockTransactionRepository.Setup(repository => repository.CalculateComissionPercent(It.IsAny<Guid>(), It.IsAny<Guid>())).ThrowsAsync(new ValidationException("Одного из аккунтов не существует"));

            // Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _transactionService.ExecuteTransaction(transaction));
            _mockTransactionRepository.Verify(repository => repository.CalculateComissionPercent(transaction.FromAccountId,
                transaction.ToAccountId), Times.Once);
            _mockConverter.VerifyNoOtherCalls();
            Assert.Equal("Одного из аккунтов не существует", exception.Message);
        }
        [Theory]
        [InlineData("USD", "RUB")]
        [InlineData("EUR", "EUR")]
        public async Task ExecuteTransaction_GetCurrencyNameReturnsCorrectNames_ShouldCallsGetRate(string fromCurrencyName, string toCurrencyName)
        {
            // Arrange
            var transaction = new Transaction()
            {
                FromAccountId = Guid.NewGuid(),
                ToAccountId = Guid.NewGuid(),
                Amount = 10
            };
            _mockAccountRepository.Setup(repository => repository.GetCurrencyName(transaction.FromAccountId)).ReturnsAsync(fromCurrencyName);
            _mockAccountRepository.Setup(repository => repository.GetCurrencyName(transaction.ToAccountId)).ReturnsAsync(toCurrencyName);

            // Act
            await _transactionService.ExecuteTransaction(transaction);

            // Assert
            _mockRatesDatabase.Verify(ratesDb => ratesDb.GetRate(fromCurrencyName, toCurrencyName), Times.Once);
            _mockAccountRepository.Verify(repository => repository.GetCurrencyName(It.IsAny<Guid>()), Times.Exactly(2));
            
        }
        [Fact]
        public async Task ExecuteTransaction_WithWrongCurrencyName_ShouldThrowException_ShouldntCallsConverter()
        {
            // Arrange
            var transaction = new Transaction()
            {
                FromAccountId = Guid.NewGuid(),
                ToAccountId = Guid.NewGuid(),
                Amount = 10
            };
            _mockRatesDatabase.Setup(ratesDb => ratesDb.GetRate(It.IsAny<String>(), It.IsAny<String>())).
                ThrowsAsync(new ValidationException("Неизвестное название валюты"));

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _transactionService.ExecuteTransaction(transaction));

            // Assert
            _mockConverter.VerifyNoOtherCalls();
            Assert.Equal("Неизвестное название валюты", exception.Message);
        }
        [Fact]
        public async Task ExecuteTransaction_WithWrongAmountInConverter_ShouldThrowException_ShouldntCallsExecuteTransaction()
        {
            // Arrange
            var transaction = new Transaction()
            {
                FromAccountId = Guid.NewGuid(),
                ToAccountId = Guid.NewGuid(),
                Amount = 10
            };
           decimal rate = 10;
            _mockRatesDatabase.Setup(r => r.GetRate(It.IsAny<String>(), It.IsAny<String>())).ReturnsAsync(rate);
            _mockConverter.Setup(converter => converter.Convert(It.IsAny<decimal>(), It.IsAny<decimal>())).
                Throws(new ValidationException("Wrong argument. Amount less than zero", new TransactionExceptionMessage()
                {
                    WrongAmount = transaction.Amount,
                }));

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _transactionService.ExecuteTransaction(transaction));

            // Assert
            _mockConverter.Verify(converter => converter.Convert(transaction.Amount, rate), Times.Once);
            _mockTransactionRepository.Verify(repository => repository.ExecuteTransaction(
                It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            Assert.Equal("Wrong argument. Amount less than zero", exception.Message);
        }
        [Theory]
        [InlineData(50, 100, 0.02)]
        [InlineData(10, 200, 0)]
        [InlineData(200, 1000, 0.05)]
        [InlineData(15, 150.28, 0.02)]
        public async Task ExecuteTransaction_CorrectCase_ShouldCallsExecuteTransactionWithCorrectParameters(
            decimal amount, decimal convertAmount, decimal percent)
        {
            // Arrange
            var transaction = new Transaction()
            {
                FromAccountId = Guid.NewGuid(),
                ToAccountId = Guid.NewGuid(),
                Amount = amount
            };
            decimal toAccountAmount = Decimal.Round(convertAmount * (1 - percent), 2);
            _mockTransactionRepository.Setup(repository => repository.CalculateComissionPercent(
                transaction.FromAccountId, transaction.ToAccountId)).ReturnsAsync(percent);
            _mockConverter.Setup(converter => converter.Convert(It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(convertAmount);
            
            // Act
            await _transactionService.ExecuteTransaction(transaction);

            // Assert
            _mockTransactionRepository.Verify(repository =>
            repository.ExecuteTransaction(transaction.Amount, toAccountAmount, transaction.FromAccountId, transaction.ToAccountId), Times.Once);
        }
        [Fact]
        public async Task ExecuteTransaction_WithWrongBalance_ShouldThrowException_ShouldntCallsSaveChangesAsync()
        {
            // Arrange
            var transaction = new Transaction()
            {
                FromAccountId = Guid.NewGuid(),
                ToAccountId = Guid.NewGuid(),
                Amount = 10
            };
        _mockTransactionRepository.Setup(repository => 
            repository.ExecuteTransaction(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<Guid>(), It.IsAny<Guid>())).
                ThrowsAsync(new ValidationException("На счете недостаточно средств для перевода", new AccountExceptionMessage
                {
                    WrongBalance = -100,
                }));

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _transactionService.ExecuteTransaction(transaction));

            // Assert
            _mockUnitOfWork.VerifyNoOtherCalls();
            Assert.Equal("На счете недостаточно средств для перевода", exception.Message);
        }
        [Fact]
        public async Task ExecuteTransaction_CorrectCase_ShouldCallsOnlyRequiredMethods()
        {
            // Arrange
            var transaction = new Transaction()
            {
                FromAccountId = Guid.NewGuid(),
                ToAccountId = Guid.NewGuid(),
                Amount = 10
            };

            // Act
            await _transactionService.ExecuteTransaction(transaction);

            // Assert
            _mockTransactionRepository.Verify(repository =>
                repository.CalculateComissionPercent(transaction.FromAccountId, transaction.ToAccountId), Times.Once);
            _mockAccountRepository.Verify(repository => repository.GetCurrencyName(It.IsAny<Guid>()), Times.Exactly(2));
            _mockRatesDatabase.Verify(ratesDb => ratesDb.GetRate(It.IsAny<String>(), It.IsAny<String>()), Times.Once);
            _mockConverter.Verify(converter => converter.Convert(It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once);
            _mockTransactionRepository.Verify(repository => repository.ExecuteTransaction(
                transaction.Amount, It.IsAny<decimal>(), transaction.FromAccountId, transaction.ToAccountId), Times.Once);
            _mockUnitOfWork.Verify(unit => unit.SaveChangesAsync(), Times.Once);
            _mockConverter.VerifyNoOtherCalls();
            _mockTransactionRepository.VerifyNoOtherCalls();
            _mockAccountRepository.VerifyNoOtherCalls();
            _mockUnitOfWork.VerifyNoOtherCalls();
            _mockRatesDatabase.VerifyNoOtherCalls();
        }
        [Fact]
        public async Task CalculateComission_WithAmountLessThanOrEqualToZero_ShouldThrowException_ShouldntCallsAnything()
        {
            // Arrange
            var validator = new InlineValidator<Transaction>();
            validator.RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Должен быть больше нуля");
            var transactionService = new TransactionService(validator, _mockUnitOfWork.Object, _mockTransactionRepository.Object,
                _mockAccountRepository.Object, _mockRatesDatabase.Object, _mockConverter.Object);
            var transaction = new Transaction()
            {
                Amount = -10
            };

            // Act
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => transactionService.CalculateComission(transaction));

            // Assert
            _mockTransactionRepository.VerifyNoOtherCalls();
            Assert.Equal("Validation failed: \r\n -- Amount: Должен быть больше нуля Severity: Error", exception.Message);
        }
        [Fact]
        public async Task CalculateComission_AccountDoesntExist_ShouldThrowException()
        {
            // Arrange
            var transaction = new Transaction()
            {
                FromAccountId = Guid.NewGuid(),
                ToAccountId = Guid.NewGuid(),
                Amount = 10
            };
            _mockTransactionRepository.Setup(repository => repository.CalculateComissionPercent(It.IsAny<Guid>(), It.IsAny<Guid>())).ThrowsAsync(new ValidationException("Одного из аккунтов не существует"));

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _transactionService.CalculateComission(transaction));

            // Assert
            _mockTransactionRepository.Verify(repository => repository.CalculateComissionPercent(
                transaction.FromAccountId, transaction.ToAccountId), Times.Once);
            Assert.Equal("Одного из аккунтов не существует", exception.Message);
        }
        [Theory]
        [InlineData(50, 0.02)]
        [InlineData(200, 0)]
        [InlineData(1000.18, 0.05)]
        [InlineData(150.28, 0.02)]
        public async Task CalculateComission_CorrectCase_ShouldReturnsCorrectToAccountAmount(decimal amount, decimal percent)
        {
            // Arrange
            var transaction = new Transaction()
            {
                FromAccountId = Guid.NewGuid(),
                ToAccountId = Guid.NewGuid(),
                Amount = amount
            };
            _mockTransactionRepository.Setup(repository =>
            repository.CalculateComissionPercent(transaction.FromAccountId, transaction.ToAccountId)).ReturnsAsync(percent);

            // Act
            var result = await  _transactionService.CalculateComission(transaction);

            // Assert
            Assert.Equal(Decimal.Round(percent * transaction.Amount, 2), result);
        }
        [Fact]
        public async Task CalculateComission_CorrectCase_ShouldCallsOneTimeCalculateComissionPercent()
        {
            // Arrange
            var transaction = new Transaction()
            {
                FromAccountId = Guid.NewGuid(),
                ToAccountId = Guid.NewGuid(),
                Amount = 10
            };

            // Act
            await _transactionService.CalculateComission(transaction);

            // Assert
            _mockTransactionRepository.Verify(repository => repository.CalculateComissionPercent(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            _mockTransactionRepository.VerifyNoOtherCalls();
            _mockConverter.VerifyNoOtherCalls();
            _mockTransactionRepository.VerifyNoOtherCalls();
            _mockAccountRepository.VerifyNoOtherCalls();
            _mockUnitOfWork.VerifyNoOtherCalls();
            _mockRatesDatabase.VerifyNoOtherCalls();
        }
    }
}
