using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    public class DeleteIngredientRequest : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Delete;

        public DeleteIngredientRequest(string id)
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/ingredients/" + id;
        }

        public static async Task<bool> SendDeleteIngredientRequest(string id)
        {
            var sendDeleteIngredientRequest = new DeleteIngredientRequest(id);
            var response = await ServiceRequestHandler.MakeServiceCall<DeleteResponse>(sendDeleteIngredientRequest);

            if(response == null)
            {
                return false;
            }
            else
            {
                var deletedIngredient = RealmManager.Find<Ingredient>(id);
                RealmManager.Write(() =>
                {
                    RealmManager.Realm.Remove(deletedIngredient);
                });
                return true;
            }
        }
    }
}
