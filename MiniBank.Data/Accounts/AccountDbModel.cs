namespace MiniBank.Data.Accounts
{
    public class AccountDbModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public decimal Balance { get; set; }
        public string CurrencyName { get; set; }
        public bool IsOpen { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
    }
}
