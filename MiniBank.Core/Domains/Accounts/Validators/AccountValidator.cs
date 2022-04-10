using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MiniBank.Core.Domains.Users.Repositories;
namespace MiniBank.Core.Domains.Accounts.Validators
{
    public class AccountValidator : AbstractValidator<Account>
    {
        public AccountValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("Не должен быть пустым");
            RuleFor(x => x.CurrencyName).NotEmpty().WithMessage("Не должен быть пустым");
            RuleFor(x => x.Balance).GreaterThanOrEqualTo(0).WithMessage("Должен быть не меньше нуля");
            RuleFor(x => x).Must(user => userRepository.IsUserExists(user.UserId).GetAwaiter().GetResult()).WithName("UserId")
                .WithMessage("Не зарегистрирован");
        }
    }
}
