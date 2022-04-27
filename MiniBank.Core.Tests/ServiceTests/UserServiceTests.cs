using Xunit;
using MiniBank.Core.Domains.Users;
using MiniBank.Core.Domains.Users.Repositories;
using MiniBank.Core.Domains.Users.Services;
using Moq;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;
using System;
using System.Collections.Generic;

namespace MiniBank.Core.Tests.ServiceTests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IValidator<User>> _mockUserValidator;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly IUserService _userService;
        public UserServiceTests()
        {
            _mockUserValidator = new Mock<IValidator<User>>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserValidator.Object, _mockUnitOfWork.Object, _mockUserRepository.Object);
        }
        [Fact]
        public async Task CreateUser_WithEmptyLogin_ShouldThrowException_ShouldCallsNothing()
        {
            // Arrange
            var validator = new InlineValidator<User>();
            validator.RuleFor(x => x.Login).NotEmpty().WithMessage("Не должен быть пустым");
            var userService = new UserService(validator, _mockUnitOfWork.Object, _mockUserRepository.Object);
            var user = new User()
            {
                Login = string.Empty,
                Email = "vova@gmail.com"
            };

            // Act
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => userService.Create(user));

            //Assert
            _mockUnitOfWork.VerifyNoOtherCalls();
            _mockUserRepository.VerifyNoOtherCalls();
            Assert.Equal("Validation failed: \r\n -- Login: Не должен быть пустым Severity: Error", exception.Message);
        }
        [Fact]
        public async Task CreateUser_UserAlreadyExists_ShouldThrowException_ShouldntCallsSaveChangesAsync()
        {
            // Arrange
            _mockUserRepository.Setup(repository => repository.Create(It.IsAny<User>())).
                ThrowsAsync(new ValidationException("Невозможно добавить пользователя. Пользователь с таким логином уже существует"));
            var user = new User()
            {
                Login = "Vova",
                Email = "vova@gmail.com"
            };

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _userService.Create(user));

            //Assert
            _mockUnitOfWork.Verify(unit => unit.SaveChangesAsync(), Times.Never);
            Assert.Equal("Невозможно добавить пользователя. Пользователь с таким логином уже существует", exception.Message);
        }
        [Theory]
        [InlineData("Vova", "vova@gmail.com")]
        [InlineData("Andrey", "andrey@yandex.ru")]
        public async Task CreateUser_CorrectUser_ShouldReturnsUser(string login, string email)
        {
            // Arrange
            var user = new User()
            {
                UserId = Guid.NewGuid(),
                Login = login,
                Email = email
            };
            _mockUserRepository.Setup(repository => repository.Create(user)).ReturnsAsync(user);

            // Act
            var result = await _userService.Create(user);

            //Assert
            Assert.Equal(user, result);
        }
        [Fact]
        public async Task CreateUser_CorrectUser_ShouldCallsOnlyCreate_And_SaveChangesAsync()
        {
            // Arrange
            var user = new User();

            // Act
            await _userService.Create(user);

            //Assert
            _mockUnitOfWork.Verify(unit => unit.SaveChangesAsync(), Times.Once);
            _mockUserRepository.Verify(repository => repository.Create(user), Times.Once);
            _mockUnitOfWork.VerifyNoOtherCalls();
            _mockUserRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task EditUser_WithEmptyLogin_ShouldThrowException_ShouldCallsNothing()
        {
            // Arrange
            var validator = new InlineValidator<User>();
            validator.RuleFor(x => x.Login).NotEmpty().WithMessage("Не должен быть пустым");
            var userService = new UserService(validator, _mockUnitOfWork.Object, _mockUserRepository.Object);
            var user = new User()
            {
                Login = string.Empty,
                Email = "vova@gmail.com"
            };

            // Act
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => userService.Edit(user));

            //Assert
            _mockUnitOfWork.VerifyNoOtherCalls();
            _mockUserRepository.VerifyNoOtherCalls();
            Assert.Equal("Validation failed: \r\n -- Login: Не должен быть пустым Severity: Error", exception.Message);
        }
        [Fact]
        public async Task EditUser_UserDoesntExist_ShouldThrowException_ShouldntCallsSaveChangesAsync()
        {
            // Arrange
            _mockUserRepository.Setup(repository => repository.Edit(It.IsAny<User>())).
                ThrowsAsync(new ValidationException("Невозможно редактировать логин и email. Пользователя с таким Id не существует"));
            var user = new User();

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _userService.Edit(user));

            //Assert
            _mockUserRepository.Verify(repository => repository.Edit(user), Times.Once);
            _mockUnitOfWork.VerifyNoOtherCalls();
            Assert.Equal("Невозможно редактировать логин и email. Пользователя с таким Id не существует", exception.Message);
        }
        [Fact]
        public async Task EditUser_CorrectUser_ShouldCallsOnlyEdit_And_SaveChangesAsync()
        {
            // Arrange
            var user = new User();

            // Act
            await _userService.Edit(user);

            //Assert
            _mockUserRepository.Verify(repository => repository.Edit(user), Times.Once);
            _mockUnitOfWork.Verify(unit => unit.SaveChangesAsync(), Times.Once);
            _mockUnitOfWork.VerifyNoOtherCalls();
            _mockUserRepository.VerifyNoOtherCalls();
        }
        [Fact]
        public async Task DeleteUser_UserDoesntExist_ShouldThrowException_ShouldntCallsSaveChangesAsync()
        {
            // Arrange
            _mockUserRepository.Setup(repository => repository.Delete(It.IsAny<Guid>())).
                ThrowsAsync(new ValidationException("Пользователя с таким Id не существует"));
            var id = new Guid();

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _userService.Delete(id));

            //Assert
            _mockUnitOfWork.VerifyNoOtherCalls();
            _mockUserRepository.Verify(repository => repository.Delete(id), Times.Once());
            Assert.Equal("Пользователя с таким Id не существует", exception.Message);
        }
        [Fact]
        public async Task DeleteUser_CorrectId_ShouldCallsOnlyDelete_And_SaveChanges()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            await _userService.Delete(id);

            //Assert
            _mockUserRepository.Verify(repository => repository.Delete(id), Times.Once);
            _mockUnitOfWork.Verify(unit => unit.SaveChangesAsync(), Times.Once);
            _mockUnitOfWork.VerifyNoOtherCalls();
            _mockUserRepository.VerifyNoOtherCalls();
        }
        [Fact]
        public async Task GetAllUsers_CorrectCase_ShouldReturnList()
        {
            // Arrange
            List<User> users = new List<User>
            {
                new User()
                {
                    UserId = Guid.NewGuid(),
                    Login = "Vova",
                    Email = "Vova@gmail.com"
                },
                new User()
                {
                    UserId = Guid.NewGuid(),
                    Login = "Sasha",
                    Email = "Sasha@gmail.com"
                }
            };
            _mockUserRepository.Setup(repository => repository.GetAll()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetAll();

            //Assert
            Assert.Equal(result, users);
        }
        [Fact]
        public async Task GetAllUsers_CorrectCase_ShouldCallsOnlyGetAll()
        {
            // Act
            await _userService.GetAll();

            //Assert
            _mockUserRepository.Verify(repository => repository.GetAll(), Times.Once);
            _mockUnitOfWork.VerifyNoOtherCalls();
            _mockUserRepository.VerifyNoOtherCalls();
        }
    }
}