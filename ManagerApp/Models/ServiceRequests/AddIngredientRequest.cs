using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    public class AddIngredientRequest : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Post;
        public AddIngredientRequestBody Body;

        public AddIngredientRequest(string itemName, string itemQuantity)
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/ingredients";
            Body = new AddIngredientRequestBody
            {
                name = itemName,
                quantity = itemQuantity
            };
        }

        public static async Task<bool> SendAddIngredientRequest(string itemName, string itemQuantity)
        {
            var sendAddIngredientRequest = new AddIngredientRequest(itemName, itemQuantity);
            var response = await ServiceRequestHandler.MakeServiceCall<PostResponse>(sendAddIngredientRequest, sendAddIngredientRequest.Body);

            if(response.message == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public class AddIngredientRequestBody
    {
        public string name;
        public string quantity;
    }
}
