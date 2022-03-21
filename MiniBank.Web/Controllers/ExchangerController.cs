using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniBank.Core;
using MiniBank.Data;
namespace MiniBank.Web.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class ExchangerController : ControllerBase
    {
        private readonly IRatesDatabase _rate;
        private readonly IConverter _converter;
        public ExchangerController(IRatesDatabase rate, IConverter converter)
        {
            this._rate = rate;
            this._converter = converter;
        }
        [HttpGet(Name = "GetValutesValue")]
        public decimal Get(int amount, string fromCurrency, string toCurrency)
        {
            return _converter.Convert(amount, _rate.GetRate(fromCurrency, toCurrency));
        }
    }
 

}
