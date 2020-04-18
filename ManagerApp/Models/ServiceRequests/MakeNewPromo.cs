using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    public class MakeNewPromo : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Post;
        public MakeNewPromoBody Body { get; set; }

        public MakeNewPromo(string couponType, string description, List<MenuItem> reqItems, List<MenuItem> appItems, string discount, string active, string repeatable)
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/coupons";

            Body = new MakeNewPromoBody();
            Body.requiredItems = new List<MenuItemId>();
            Body.appliedItems = new List<MenuItemId>();

            foreach (MenuItem menuItem in reqItems)
            {
                MenuItemId menuItemId = new MenuItemId
                {
                    _id = menuItem._id
                };
                Body.requiredItems.Add(menuItemId);
            }

            foreach(MenuItem menuItem in appItems)
            {
                MenuItemId menuItemId = new MenuItemId
                {
                    _id = menuItem._id
                };
                Body.appliedItems.Add(menuItemId);
            }

            Body.active = active.ToLower();
            Body.repeatable = repeatable.ToLower();
            Body.couponType = couponType;
            Body.discount = discount;
            Body.description = description;
        }

        public async static Task<bool> SendMakeNewPromo(string couponType, string description, List<MenuItem> reqItems, List<MenuItem> appItems, string discount, string active, string repeatable)
        {
            MakeNewPromo makeNewPromo = new MakeNewPromo(couponType, description, reqItems, appItems, discount, active, repeatable);
            var response = await ServiceRequestHandler.MakeServiceCall<MakeNewPromoResponse>(makeNewPromo, makeNewPromo.Body);

            if(response.message != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class MakeNewPromoResponse
    {
        public string message { get; set; }
        public string _id { get; set; }
    }

    public class MenuItemId
    {
        public string _id { get; set; }
    }

    public class MakeNewPromoBody
    {
        public string couponType { get; set; }
        public List<MenuItemId> requiredItems { get; set; }
        public List<MenuItemId> appliedItems { get; set; }
        public string discount { get; set; }
        public string active { get; set; }
        public string repeatable { get; set; }
        public string description { get; set; }
    }
}
