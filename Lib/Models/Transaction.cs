using System;

namespace Goalie.Lib.Models
{
    public class Transaction : UniqueModel
    {
        public Transaction()
        {
            Date = DateTime.Now;
        }
        public string Description { get; set; }
        public string OtherAccountID { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
