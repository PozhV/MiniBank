using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MiniBank.Core.Domains.Accounts.Repositories;
namespace MiniBank.Core.Domains.Transactions.Validators
{
    public class TransactionValidator : AbstractValidator<Transaction>
    {
        public TransactionValidator()
        {
            RuleFor(x => x.FromAccountId).NotEmpty().WithMessage("Не должен быть пустым");
            RuleFor(x => x.ToAccountId).NotEmpty().WithMessage("Не должен быть пустым");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Должен быть больше нуля");
            RuleFor(x => x).Must(user => user.ToAccountId != user.FromAccountId).WithName("AccountsId")
                .WithMessage("Не должны совпадать");
        }
    }
}
