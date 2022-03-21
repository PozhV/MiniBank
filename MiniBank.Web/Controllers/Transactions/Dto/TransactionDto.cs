namespace MiniBank.Web.Controllers.Transactions.Dto
{
    public class TransactionDto
    {
        public decimal Amount { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
    }
}
