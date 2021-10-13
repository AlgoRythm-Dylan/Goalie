using System.Collections.Generic;

namespace Goalie.Lib.Models
{
    public class Profile
    {
        public Profile()
        {
            Name = "";
        }
        public string ID { get; set; }
        public string Name { get; set; }
        public List<string> Accounts { get; set; }
    }
}
