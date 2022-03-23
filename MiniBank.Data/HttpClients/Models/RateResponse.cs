namespace MiniBank.Data.HttpClients.Models
{
    public class RateResponse
    {
        public DateTime Date { get; set; }
        public Dictionary<string, CurrencyItem> Valute { get => CurrencyRates; set => CurrencyRates = value; }
        public Dictionary<string, CurrencyItem> CurrencyRates { get; set; }
    }
    public class CurrencyItem
    {
        public string ID { get; set; }
        public string NumCode { get; set; }
        public double Rate { get; set; }
        public double Value { get => Rate; set => Rate = value; }
    }
}
