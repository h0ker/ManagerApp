using Realms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models
{
    public class MenuItem : RealmObject
    {
        public IList<Ingredient> ingredients { get; }
        public bool prepared { get; set; }
        public bool paid { get; set; }
        public string special_instruct { get; set; }
        public string _id { get; set; }
        public string name { get; set; }
        public Picture picture { get; set; }
        public string description { get; set; }
        public double price { get; set; }
        public string nutrition { get; set; }
        public string item_name { get; set; }
        public string category { get; set; }
    }
}
