using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models
{
    public class Picture : RealmObject
    {
        public string type { get; set; }
        public string data { get; set; }
    }
}
