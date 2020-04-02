using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    public class GetIngredientsRequest : ServiceRequest
    {
        //the endpoint we are trying to hit
        public override string Url { get; set; }
        //the type of request
        public override HttpMethod Method => HttpMethod.Get;

        public GetIngredientsRequest()
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/ingredients";
        }
        public static async Task<bool> SendGetIngredientsRequest()
        {
            //make a new request object
            var serviceRequest = new GetIngredientsRequest();
            //get a response
            var response = await ServiceRequestHandler.MakeServiceCall<IngredientList>(serviceRequest);

            if(response == null)
            {
                //call failed
                return false;
            }
            else
            {
                RealmManager.RemoveAll<IngredientList>();
                //add the response into the local database
                RealmManager.AddOrUpdate<IngredientList>(response);
                //call succeeded
                return true;
            }
        }
    }
}
