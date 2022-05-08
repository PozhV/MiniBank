using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBank.Core
{
    public interface IConverter
    {
        decimal Convert(decimal amount, decimal rate);
    }
}
