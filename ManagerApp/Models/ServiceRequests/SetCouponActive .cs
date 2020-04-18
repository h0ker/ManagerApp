using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    public class SetCouponActive : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Put;
        public List<SetCouponActiveBody> Body { get; set; }

        public SetCouponActive(string id, bool active)
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/coupons/" + id;
            Body = new List<SetCouponActiveBody>();
            SetCouponActiveBody setCouponActiveBody = new SetCouponActiveBody
            {
                propName = "active",
                value = active
            };
            Body.Add(setCouponActiveBody);
        }

        public async static Task<bool> SendSetCouponActive(string id, bool active)
        {
            SetCouponActive setCouponActive = new SetCouponActive(id, active);
            var response = await ServiceRequestHandler.MakeServiceCall<SetCouponActiveResponse>(setCouponActive, setCouponActive.Body);
            if (response.nModified == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class SetCouponActiveResponse
    {
        public int nModified { get; set; }
    }

    public class SetCouponActiveBody
    {
        public string propName { get; set; } 
        public bool value { get; set; }
    }
}
