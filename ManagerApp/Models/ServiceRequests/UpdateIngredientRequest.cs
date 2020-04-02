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
        public override HttpMethod Method => HttpMethod.Post;
        public IList<UpdateIngredientRequestObject> Body;

        public UpdateIngredientRequest(string oldValue, string newValue)
        {
            UpdateIngredientRequestObject updateIngredientRequestObject = new UpdateIngredientRequestObject
            {
                propName = oldValue,
                value = newValue
            };

            Body.Add(updateIngredientRequestObject);
        }
    }

    public class UpdateIngredientRequestObject
    {
        public string propName { get; set; }
        public string value { get; set; }
    }
}
