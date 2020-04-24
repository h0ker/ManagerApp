using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models
{
    class HotItem : RealmObject
    {
        public string createdAt { get; set; }
        [PrimaryKey]
        public string category { get; set; }
        public string _id { get; set; }

    }
}
