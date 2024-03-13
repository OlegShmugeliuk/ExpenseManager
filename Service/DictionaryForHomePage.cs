using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
	public class DictionaryForHomePage
	{
        public string ExpenditureType { get; set; }
        public double TotalPrice { get; set; }
        public string CurrencyType { get; set; }
        
        public DateTime? PostDate { get; set; } = DateTime.Now;


        public Dictionary<string, decimal> ExpenditureDictionary { get; set; }
	}
}
