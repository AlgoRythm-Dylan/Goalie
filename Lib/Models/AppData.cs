namespace Goalie.Lib.Models
{
    public class AppData
    {
        public AppData()
        {
            CurrentTransactionID = 1;
        }
        public int CurrentTransactionID { get; set; }
        public string CurrentProfileID { get; set; }
    }
}
