using Xunit;
using MiniBank.Core.Domains.Users.Validators;
using MiniBank.Core.Domains.Users;
using Moq;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;
using System;
using FluentValidation.Results;


namespace MiniBank.Core.Tests.ValidatorTests
{
    public class UserValidatorTests
    {
        private readonly IValidator<User> _userValidator;
        public UserValidatorTests()
        {
            _userValidator = new UserValidator();
        }
        [Fact]
        public async Task UserValidator_WithNullEmail_ShouldThrowException()
        {
            // Arrange
            var user = new User()
            {
                Login = "Vova",
                Email = string.Empty
            };

            // Act
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _userValidator.ValidateAndThrowAsync(user));

            // Assert
            Assert.Equal("Validation failed: \r\n -- Email: Не должен быть пустым Severity: Error", exception.Message);
        }
        [Fact]
        public async Task UserValidator_WithNullLogin_ShouldThrowException()
        {
            // Arrange
            var user = new User()
            {
                Login = string.Empty,
                Email = "string"
            };
            
            // Act
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _userValidator.ValidateAndThrowAsync(user));

            // Assert
            Assert.Equal("Validation failed: \r\n -- Login: Не должен быть пустым Severity: Error", exception.Message);
        }
        [Theory]
        [InlineData("AAAAAAAAAAAAAAAAAAAAAAAA")]
        [InlineData("SmirnovNickolayNickolaevich")]
        public async Task UserValidator_WithLongLogin_ShouldThrowException(string login)
        {
            // Arrange
            var user = new User()
            {
                Login = login,
                Email = "string"
            };
            
            // Act
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _userValidator.ValidateAndThrowAsync(user));
            
            // Assert
            Assert.Equal("Validation failed: \r\n -- Login.Length: Не должен быть длиннее 20 символов Severity: Error", exception.Message);
        }
        [Theory]
        [InlineData("Vova", "vova@gmail.com")]
        [InlineData("Smirnov", "Smirnov@yandex.ru")]
        public async Task UserValidator_WithCorrectParameters_ShouldNotThrowException(string login, string email)
        {
            // Arrange
            var user = new User()
            {
                Login = login,
                Email = email
            };
            
            // Act
            var exception = await Record.ExceptionAsync(() => _userValidator.ValidateAndThrowAsync(user));
            
            // Assert
            Assert.Null(exception);
        }
    }
}
