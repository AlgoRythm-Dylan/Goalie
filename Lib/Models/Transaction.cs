namespace Goalie.Lib.Models
{
    public class Transaction : UniqueModel
    {
        public string Description { get; set; }
        public string OtherAccountID { get; set; }
        public decimal Amount { get; set; }
    }
}
