using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realms;

namespace ManagerApp.Models
{
    public class OrderList : RealmObject
    {
        //creating a list of orders
        public IList<Order> orders { get; }
    }
}
