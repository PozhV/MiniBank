using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace MiniBank.Data
{
    
    public interface IRatesDatabase
    {
        void CheckCurrencyName(string currencyName);
        decimal GetRate(string fromCurrency, string toCerrency);
    }
}
