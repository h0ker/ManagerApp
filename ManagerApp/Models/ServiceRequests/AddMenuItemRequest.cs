using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    public class AddMenuItemRequest : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Post;
        public AddMenuItemRequestBody Body { get; set; }

        public AddMenuItemRequest(List<string> ingredients, string name, string picture, string description, double price, string nutrition, string item_type, string category)
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/menuitems";

            Body = new AddMenuItemRequestBody
            {
                ingredients = ingredients,
                name = name,
                picture = picture,
                description = description,
                price = price,
                nutrition = nutrition,
                item_type = item_type,
                category = category
            };
        }

        public async static Task<bool> SendAddMenuItemRequest(List<string> ingredients, string name, string picture, string description, double price, string nutrition, string item_type, string category)
        {
            AddMenuItemRequest addMenuItemRequest = new AddMenuItemRequest(ingredients, name, picture, description, price, nutrition, item_type, category);
            var response = await ServiceRequestHandler.MakeServiceCall<AddMenuItemRequestResponse>(addMenuItemRequest, addMenuItemRequest.Body);
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

    public class AddMenuItemRequestBody
    {
        public List<string> ingredients { get; set; }
        public string name { get; set; }
        public string picture { get; set; }
        public string description { get; set; }
        public double price { get; set; }
        public string nutrition { get; set; }
        public string item_type { get; set; }
        public string category { get; set; }
    }

    public class AddMenuItemRequestResponse
    {
        public string message { get; set; }
    }
}
