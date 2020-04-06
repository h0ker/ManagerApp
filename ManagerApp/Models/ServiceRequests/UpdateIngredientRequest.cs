using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    public class UpdateIngredientRequest : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Put;
        public IList<UpdateIngredientRequestObject> Body;

        public UpdateIngredientRequest(string id, string oldValue, string newValue)
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/Ingredients/" + id;

            UpdateIngredientRequestObject updateIngredientRequestObject = new UpdateIngredientRequestObject
            {
                propName = oldValue,
                value = newValue
            };
            Body = new List<UpdateIngredientRequestObject>();
            Body.Add(updateIngredientRequestObject);
        }

        public static async Task<bool> SendUpdateIngredientRequest(string id, string oldValue, string newValue)
        {
            var sendUpdateIngredientRequest = new UpdateIngredientRequest(id, oldValue, newValue);
            var response = await ServiceRequestHandler.MakeServiceCall<DeleteResponse>(sendUpdateIngredientRequest, sendUpdateIngredientRequest.Body);

            if(response == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public class UpdateIngredientRequestObject
    {
        public string propName { get; set; }
        public string value { get; set; }
    }
}
