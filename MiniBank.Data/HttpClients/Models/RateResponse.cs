namespace MiniBank.Data.HttpClients.Models
{
    public class RateResponse
    {
        public DateTime Date { get; set; }
        public Dictionary<string, ValuteItem> Valute { get; set; }
    }
    public class ValuteItem
    {
        public string ID { get; set; }
        public string NumCode { get; set; }
        public double Value { get; set; }
    }
}
