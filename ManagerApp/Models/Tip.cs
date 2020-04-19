using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models
{
    class Tip : RealmObject
    {
        public string _id { get; set; }
        public string employee_id { get; set; }
        public double tip_amount { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
    }
}
