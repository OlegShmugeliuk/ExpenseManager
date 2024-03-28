using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class InfoBody
    {
        public int Id { get; set; }
        public string? Body { get ; set; }
        public string? ExpenditureType { get; set; }
        public string? CurrentyType { get; set; }
        public double Price { get; set; }        
        public DateTime? postDateTime { get; set; } =  DateTime.Now;

        public string User {  get; set; }

        public InfoBody() {}
        public InfoBody(InfoBody odj)
        {
            Body = odj.Body;
            ExpenditureType = odj.ExpenditureType;
            CurrentyType = odj.CurrentyType;
            Price = odj.Price;
            postDateTime = odj.postDateTime;
        }
    }
}
