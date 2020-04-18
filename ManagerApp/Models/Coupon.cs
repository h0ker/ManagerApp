using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models
{
    public class Coupon : RealmObject
    {
        public IList<string> requiredItems { get; }
        public IList<string> appliedItems { get; }
        public bool active { get; set; }
        [PrimaryKey]
        public string _id { get; set; }
        public string couponType { get; set; }
        public int discount { get; set; }
        public bool repeatable { get; set; }
        public string description { get; set; }
    }
}
