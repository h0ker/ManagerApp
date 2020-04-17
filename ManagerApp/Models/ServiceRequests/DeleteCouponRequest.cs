using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    public class DeleteCouponRequest : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Delete;

        public DeleteCouponRequest(string id)
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/coupons/" + id;
        }

        public async static Task<bool> SendDeleteCouponRequest(string id)
        {
            DeleteCouponRequest deleteCouponRequest = new DeleteCouponRequest(id);
            var response = await ServiceRequestHandler.MakeServiceCall<DeleteCouponResponse>(deleteCouponRequest);
            if (response.message != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class DeleteCouponResponse
    {
        public string message { get; set; }
    }
}
