using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goalie.Lib.Models
{
    public class UniqueModel
    {
        public string ID { get; set; }
        public void NewID()
        {
            ID = Data.ServiceBase.NewID();
        }
    }
}
