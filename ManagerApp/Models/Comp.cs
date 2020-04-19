using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models
{
    public class Comp : RealmObject
    {
        public string _id { get; set; }
        public string employee_id { get; set; }
        public string menuItem_id { get; set; }
        public string reason { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        //public string menuItemName => RealmManager.Find<MenuItem>(menuItem_id).name;
    }
}
