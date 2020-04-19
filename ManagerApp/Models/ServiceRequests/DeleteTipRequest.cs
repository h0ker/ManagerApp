using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    public class DeleteTipRequest : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Delete;

        public DeleteTipRequest(string id)
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/tips/" + id;
        }

        public async static Task<bool> SendDeleteTipRequest(string id)
        {
            DeleteTipRequest deleteTipRequest = new DeleteTipRequest(id);
            var response = await ServiceRequestHandler.MakeServiceCall<DeleteTipResponse>(deleteTipRequest);
            if(response.deletedCount == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class DeleteTipResponse
    {
        public int deletedCount { get; set; }
    }
}
