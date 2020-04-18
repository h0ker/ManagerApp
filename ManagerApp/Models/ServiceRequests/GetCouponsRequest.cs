using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    public class GetCouponsRequest : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Get;

        public GetCouponsRequest()
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/coupons";
        }

        public async static Task<bool> SendGetCouponsRequest()
        {
            GetCouponsRequest getCouponsRequest = new GetCouponsRequest();
            var response = await ServiceRequestHandler.MakeServiceCall<CouponList>(getCouponsRequest);
            if(response != null)
            {
                RealmManager.RemoveAll<CouponList>();
                RealmManager.AddOrUpdate<CouponList>(response);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
