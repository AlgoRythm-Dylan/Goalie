using System.Collections.Generic;

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
        public List<Account> Accounts { get; set; }
    }
}
