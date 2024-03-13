using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Currenty
{
    public class ConvertValue
    {
        
        public double ConvertFrom(string type, Dictionary<string, double> ExchangeRate, double value)
        {
            double result;

            result = value / ExchangeRate[type];

            if(result < 0 || result==0) {
                return 0;
            }
            return result;
        }

    }
}
