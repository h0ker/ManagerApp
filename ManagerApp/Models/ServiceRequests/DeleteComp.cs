using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    public class DeleteComp : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Delete;

        public DeleteComp(string id)
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/comps/" + id;
        }

        public async static Task<bool> SendDeleteComp(string id)
        {
            DeleteComp deleteComp = new DeleteComp(id);
            var response = await ServiceRequestHandler.MakeServiceCall<DeleteCompResponse>(deleteComp);
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

    public class DeleteCompResponse
    {
        public int deletedCount { get; set; }
    }
}
