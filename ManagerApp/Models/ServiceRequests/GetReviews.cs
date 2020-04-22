using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    public class GetReviews : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Get;

        public GetReviews()
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/reviews";
        }

        public async static Task<bool> SendGetReviews()
        {
            GetReviews getReviews = new GetReviews();
            var response = await ServiceRequestHandler.MakeServiceCall<ReviewList>(getReviews);
            if(response != null)
            {
                RealmManager.RemoveAll<ReviewList>();
                RealmManager.RemoveAll<Review>();
                RealmManager.AddOrUpdate<ReviewList>(response);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
