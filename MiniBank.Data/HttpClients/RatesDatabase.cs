using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using MiniBank.Data.HttpClients.Models;
using System.Text.Json;
using MiniBank.Core;
using MiniBank.Core.Domains.Users.UserMessages;
namespace MiniBank.Data
{
    public class RatesDatabase : IRatesDatabase
    {
        private readonly HttpClient _httpClient;
        private static List<string> ValideCurrencyNames = new List<string> { "USD", "RUB", "EUR" };
        public RatesDatabase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public void CheckCurrencyName(string currency)
        {
            var fromEntity = ValideCurrencyNames.FirstOrDefault(it => it == currency);
            if (fromEntity == null)
            {
                throw new ValidationException("Неизвестное название валюты", new CurrencyExceptionMessage()
                {
                    WrongCurrency = currency
                });
            }
        }
        public decimal GetRate(string fromCurrency, string toCurrency)
        {
            CheckCurrencyName(fromCurrency);
            CheckCurrencyName(toCurrency);
            var response = _httpClient.GetFromJsonAsync<RateResponse>("daily_json.js")
      .GetAwaiter().GetResult();
            response.Valute.Add("RUB", new ValuteItem() { Value = 1 });
            return Convert.ToDecimal(response.Valute[toCurrency].Value / response.Valute[fromCurrency].Value);
        }
    }
}