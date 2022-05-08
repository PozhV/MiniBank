using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
namespace MiniBank.Core.Domains.Users.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Login).NotNull().WithMessage("Не должен быть пустым");
            RuleFor(x => x.Email).NotNull().WithMessage("Не должен быть пустым");
            RuleFor(x => x.Login).NotEmpty().WithMessage("Не должен быть пустым");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Не должен быть пустым");
            RuleFor(x => x.Login.Length).LessThanOrEqualTo(20).WithMessage("Не должен быть длиннее 20 символов");

        }
    }
}
