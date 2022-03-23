namespace MiniBank.Data.Accounts
{
    public class AccountDbModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
        public string CurrencyName { get; set; }
        public bool IsOpen { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
    }
}
