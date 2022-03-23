namespace MiniBank.Web.Controllers.Transactions.Dto
{
    public class TransactionDto
    {
        public decimal Amount { get; set; }
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
    }
}
