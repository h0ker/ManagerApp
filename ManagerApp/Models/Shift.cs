using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models
{
    public class Shift : RealmObject
    {
        public string _id { get; set; }
        public string employee_id { get; set; }
        public string clock_in { get; set; }
        public string clock_out { get; set; }
    }
}
