using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrader.Service.Services.Trading.Indicators
{
    public interface IIndicator
    {
        void OnTick();
        void Calculate();
    }
}
