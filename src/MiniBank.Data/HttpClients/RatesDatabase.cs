using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MiniBank.Data.HttpClients.Models;
using MiniBank.Core;
using MiniBank.Core.Domains.Users.UserMessages;
namespace MiniBank.Data
{
    public class RatesDatabase : IRatesDatabase
    {
        private readonly HttpClient _httpClient;
        private static List<string> _valideCurrencyNames = new List<string> { "USD", "RUB", "EUR" };
        public RatesDatabase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public void CheckCurrencyName(string currency)
        {
            if (!_valideCurrencyNames.Any(it => it == currency))
            {
                throw new ValidationException("Неизвестное название валюты", new CurrencyExceptionMessage()
                {
                    WrongCurrency = currency
                });
            }
        }
        public async Task<decimal> GetRate(string fromCurrency, string toCurrency)
        {
            CheckCurrencyName(fromCurrency);
            CheckCurrencyName(toCurrency);
            var response = await _httpClient.GetFromJsonAsync<RateResponse>("daily_json.js");
            response.Valute.Add("RUB", new CurrencyItem { Rate = 1 });
            return Convert.ToDecimal(response.CurrencyRates[toCurrency].Rate / response.CurrencyRates[fromCurrency].Rate);
        }
    }
}