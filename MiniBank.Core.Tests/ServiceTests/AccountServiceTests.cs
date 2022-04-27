using Xunit;
using MiniBank.Core.Domains.Accounts;
using MiniBank.Core.Domains.Accounts.Repositories;
using MiniBank.Core.Domains.Accounts.Services;
using Moq;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;
using System;
using System.Collections.Generic;

namespace MiniBank.Core.Tests.ServiceTests
{
    public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Mock<IValidator<Account>> _mockAccountValidator;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly IAccountService _accountService;
        public AccountServiceTests()
        {
            _mockAccountValidator = new Mock<IValidator<Account>>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockAccountRepository = new Mock<IAccountRepository>();
            _accountService = new AccountService(_mockAccountValidator.Object, _mockUnitOfWork.Object, _mockAccountRepository.Object);
        }
        [Fact]
        public async Task CreateAccount_WithEmptyCurrencyName_ShouldThrowException_And_CallsNothing()
        {
            //Arrange
            var validator = new InlineValidator<Account>();
            validator.RuleFor(x => x.Balance).GreaterThanOrEqualTo(0).WithMessage("Должен быть не меньше нуля");
            var accountService = new AccountService(validator, _mockUnitOfWork.Object, _mockAccountRepository.Object);
            var account = new Account()
            {
                UserId = Guid.NewGuid(),
                CurrencyName = "USD",
                Balance = -10
            };

            //Act
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => accountService.Create(account));
            
            //Assert
            _mockUnitOfWork.VerifyNoOtherCalls();
            _mockAccountRepository.VerifyNoOtherCalls();
            Assert.Equal("Validation failed: \r\n -- Balance: Должен быть не меньше нуля Severity: Error", exception.Message);
        }
        [Fact]
        public async Task CreateAccount_UserAlreadyExists_ShouldThrowException_ShouldntCallsSaveChangesAsync()
        {
            //Arrange
            var account = new Account();
            _mockAccountRepository.Setup(repository => repository.Create(It.IsAny<Account>())).
                ThrowsAsync(new ValidationException("Пользователя с таким Id не существует"));
            
            //Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _accountService.Create(account));
            
            //Assert
            _mockAccountRepository.Verify(repository => repository.Create(account), Times.Once);
            _mockUnitOfWork.Verify(unit => unit.SaveChangesAsync(), Times.Never);
            Assert.Equal("Пользователя с таким Id не существует", exception.Message);
        }
        [Theory]
        [InlineData(100, "RUB")]
        [InlineData(0, "USD")]
        public async Task CreateAccount_WithCorrectParameters_ShouldReturnsAccount(decimal balance, string currencyName)
        {
            //Arrange
            var account = new Account()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Balance = balance,
                CurrencyName = currencyName,
                IsOpen = true
            };
            _mockAccountRepository.Setup(repository => repository.Create(account)).ReturnsAsync(account);

            //Act
            var result = await _accountService.Create(account);

            //Assert
            Assert.Equal(result, account);
        }
        [Fact]
        public async Task CreateAccount_CorrectCase_ShouldCallsCreate_ShouldCallsSaveChangesAsync()
        {
            //Arrange
            var account = new Account();

            //Act
            await _accountService.Create(account);

            //Assert
            _mockUnitOfWork.Verify(unit => unit.SaveChangesAsync(), Times.Once);
            _mockAccountRepository.Verify(repository => repository.Create(account), Times.Once);
            _mockAccountRepository.VerifyNoOtherCalls();
            _mockUnitOfWork.VerifyNoOtherCalls();
        }
        [Fact]
        public async Task DeleteAccount_UserDoesntExist_ShouldThrowException()
        {
            //Arrange
            var id = Guid.NewGuid();
            _mockAccountRepository.Setup(repository => repository.Delete(It.IsAny<Guid>())).ThrowsAsync(new ValidationException("Аккаунта с таким Id не существует"));

            //Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _accountService.Delete(id));

            //Assert
            _mockUnitOfWork.Verify(unit => unit.SaveChangesAsync(), Times.Never);
            Assert.Equal("Аккаунта с таким Id не существует", exception.Message);
        }
        [Fact]
        public async Task DeleteAccount_WithCorrectParameters_ShouldCallsOnlyRequiredMethods()
        {
            //Arrange
            var id = Guid.NewGuid();

            //Act
            await _accountService.Delete(id);

            //Assert
            _mockAccountRepository.Verify(repository => repository.Delete(id), Times.Once);
            _mockUnitOfWork.Verify(unit => unit.SaveChangesAsync(), Times.Once);
            _mockAccountRepository.VerifyNoOtherCalls();
            _mockUnitOfWork.VerifyNoOtherCalls();

        }
        [Fact]
        public async Task GetAllAccounts_CorrectCase_ShouldReturnList_And_CallsOnlyGetAll()
        {
            //Arrange
            List<Account> accounts = new List<Account>
            {
                new Account()
                {
                    UserId = new Guid(),
                    Balance = 0,
                    CurrencyName = "RUB",
                    IsOpen = true
                },
                new Account()
                {
                    UserId = new Guid(),
                    Balance = 100,
                    CurrencyName = "USD",
                    IsOpen = true
                }
            };
            _mockAccountRepository.Setup(repository => repository.GetAll()).ReturnsAsync(accounts);

            //Act
            var result = await _accountService.GetAll();

            //Assert
            Assert.Equal(result, accounts);
            _mockAccountRepository.Verify(repository => repository.GetAll(), Times.Once());
            _mockUnitOfWork.VerifyNoOtherCalls();
            _mockAccountRepository.VerifyNoOtherCalls();
        }
    }
}
