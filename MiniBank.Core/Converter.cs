using MiniBank.Core.Domains.Users.UserMessages;
namespace MiniBank.Core
{
    public class Converter : IConverter
    {
        public decimal Convert(decimal amount, decimal rate)
        {
            if (amount < 0)
               throw new ValidationException("Wrong argument. Amount less than zero:", new TransactionExceptionMessage()
               {
                   WrongAmount = amount,
               });
            return Decimal.Round(amount / rate, 2);
        }
    }
}