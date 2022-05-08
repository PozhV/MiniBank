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

        public AccountValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("Не должен быть пустым");
            RuleFor(x => x.CurrencyName.Length).Equal(3).WithMessage("Должна быть равна 3");
            RuleFor(x => x.CurrencyName).Must(name => name == name.ToUpper()).WithMessage("Должно состоять только из заглавных букв");
            RuleFor(x => x.Balance).GreaterThanOrEqualTo(0).WithMessage("Должен быть не меньше нуля");
        }
    }
}
