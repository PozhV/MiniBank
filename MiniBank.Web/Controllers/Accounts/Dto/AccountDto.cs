namespace MiniBank.Web.Controllers.Accounts.Dto
{
    public class AccountDto
    {
        public Guid UserId { get; set; }
        public string CurrencyName { get; set; }
        public decimal Balance { get; set; }
    }
}
