using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniBank.Core.Domains.Users.UserMessages;
namespace MiniBank.Core
{
    public class ValidationException : Exception
    {
        //public decimal _value { get; }
        //public string _currencyName { get; }
        public AccountExceptionMessage _accountExceptionMessage;
        public TransactionExceptionMessage _transactionExceptionMessage;
        public CurrencyExceptionMessage _currencyExceptionMessage;
        public ValidationException(string message) : base(message)
        {
        }
        public ValidationException(string message, AccountExceptionMessage accountExceptionMessage):base(message)
        {
            _accountExceptionMessage = accountExceptionMessage;
        }
        public ValidationException(string message, TransactionExceptionMessage transactionExceptionMessage) : base(message)
        {
            _transactionExceptionMessage = transactionExceptionMessage;
        }
        public ValidationException(string message, CurrencyExceptionMessage currencyExceptionMessage) : base(message)
        {
            _currencyExceptionMessage = currencyExceptionMessage;
        }
        /*/public ValidationException(string message, decimal amount) : base(message)
        {
            _value = amount;
        }
        public ValidationException(string message, string valuteName) : base(message)
        {
            _currencyName = valuteName;
        }/*/
    }
}
