using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class IncomePerson
    {
        public int? Id { get; set; } = new Random().Next();
        public double? Income { get; set; } = 0.0;
        public string Currency { get; set; }
        public  string SourceOfIncome { get; set; }
        public DateTime? IncomeTime { get; set; } = DateTime.Now;

        public IncomePerson() { }

        public IncomePerson(IncomePerson incomePerson)
        {            
            Income = incomePerson.Income;  
            Currency = incomePerson.Currency;
            SourceOfIncome = incomePerson.SourceOfIncome;
            Income = incomePerson.Income;
        }
	}
}
