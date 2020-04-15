using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    public class DeleteMenuItemRequest : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Delete;

        public DeleteMenuItemRequest(string id)
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/menuitems/" + id;
        }

        public async static Task<bool> SendDeleteMenuItemRequest(string id)
        {
            DeleteMenuItemRequest deleteMenuItemRequest = new DeleteMenuItemRequest(id);
            var response = await ServiceRequestHandler.MakeServiceCall<DeleteMenuItemRequestResponse>(deleteMenuItemRequest);
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

    public class DeleteMenuItemRequestResponse
    {
        public string message { get; set; }
    }
}
