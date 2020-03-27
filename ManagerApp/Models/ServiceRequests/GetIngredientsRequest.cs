using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    class GetIngredientsRequest : ServiceRequest
    {
        public override string Url => "https://dijkstras-steakhouse-restapi.herokuapp.com/ingredients";
        public override HttpMethod Method => HttpMethod.Get;
        public override Dictionary<string, string> Headers => null;

        public static async Task<bool> SendGetIngredientsRequest()
        {
            var serviceRequest = new GetIngredientsRequest();
            var response = await ServiceRequestHandler.MakeServiceCall<IngredientList>(serviceRequest);

            if(response == null)
            {
                return false;
            }
            else
            {
                RealmManager.AddOrUpdate<IngredientList>(response);
                return true;
            }
        }
    }
}
