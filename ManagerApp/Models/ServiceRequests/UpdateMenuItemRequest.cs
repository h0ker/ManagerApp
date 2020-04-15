using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    public class UpdateMenuItemRequest : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Put;
        public IList<UpdateMenuItemRequestObject> Body { get; set; }

        public UpdateMenuItemRequest(string id, string category, double price, string name, string nutrition, string description)
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/menuitems/" + id;
            Body = new List<UpdateMenuItemRequestObject>();

            UpdateMenuItemRequestObject updateMenuItemRequestObject;
            updateMenuItemRequestObject = new UpdateMenuItemRequestObject
            {
                propName = "category",
                value = category
            };

            Body.Add(updateMenuItemRequestObject);

            updateMenuItemRequestObject = new UpdateMenuItemRequestObject
            {
                propName = "price",
                value = price.ToString()
            };

            Body.Add(updateMenuItemRequestObject);

            updateMenuItemRequestObject = new UpdateMenuItemRequestObject
            {
                propName = "name",
                value = name
            };

            Body.Add(updateMenuItemRequestObject);

            updateMenuItemRequestObject = new UpdateMenuItemRequestObject
            {
                propName = "nutrition",
                value = nutrition
            };

            Body.Add(updateMenuItemRequestObject);

            updateMenuItemRequestObject = new UpdateMenuItemRequestObject
            {
                propName = "description",
                value = description
            };

            Body.Add(updateMenuItemRequestObject);
        }

        public async static Task<bool> SendUpdateMenuItemRequest(string id, string category, double price, string name, string nutrition, string description)
        {
            UpdateMenuItemRequest updateMenuItemRequest = new UpdateMenuItemRequest(id, category, price, name, nutrition, description);
            var response = await ServiceRequestHandler.MakeServiceCall<DeleteResponse>(updateMenuItemRequest, updateMenuItemRequest.Body);
            if(response.electionId != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class UpdateMenuItemRequestObject
    {
        public string propName { get; set; }
        public string value { get; set; }
    }
}
