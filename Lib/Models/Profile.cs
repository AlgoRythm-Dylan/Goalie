using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Goalie.Lib.Models
{
    public class Profile : UniqueModel
    {
        public Profile()
        {
            Name = "";
            Accounts = new List<Account>();
        }
        public string Name { get; set; }
        public string PinnedAccount { get; set; }
        public List<Account> Accounts { get; set; }
        // Stats
        public int PaychecksAllTime { get; set; }
        public decimal IncomeAllTime { get; set; }

        public Account GetAccountByID(string ID)
        {
            foreach (Account account in Accounts)
                if (account.ID == ID)
                    return account;
            return null;
        }
        [JsonIgnore]
        public Account GeneralAccount { 
            get
            {
                foreach (Account account in Accounts)
                    if (account.Type == AccountType.GeneralSavings)
                        return account;
                return null;
            } 
        }

    }
}
