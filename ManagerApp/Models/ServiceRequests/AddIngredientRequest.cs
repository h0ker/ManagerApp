using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    //This is an example of a POST request
    public class AddIngredientRequest : ServiceRequest
    {
        //It has a URL like usual, check the API Anthony made if there are concerns here
        public override string Url { get; set; }
        //This is the type of request, which is a POST
        public override HttpMethod Method => HttpMethod.Post;
        //This is the main difference between GET and POST. POST requests have a BODY that you must send
        public AddIngredientRequestBody Body;

       /*
        * This is a constructor for the request object that fills the body with data.
        * if the way the URL is set looks different, that's just because I changed the way
        * it populates, but it's functionally the same as what is in the other apps. This way
        * just allows for you to add extensions to the URL, such as a specific object ID
        */
        public AddIngredientRequest(string itemName, string itemQuantity)
        {
            //new location for the endpoint you're hitting
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/ingredients";

            //make a new instance of your POST BODY (found below in this file)
            Body = new AddIngredientRequestBody
            {
                name = itemName,
                quantity = itemQuantity
            };
        }

        public static async Task<bool> SendAddIngredientRequest(string itemName, string itemQuantity)
        {
            //call to the constructor above
            var sendAddIngredientRequest = new AddIngredientRequest(itemName, itemQuantity);
            //call to ServiceRequestHandler, arguments are the request object (like usual), but notice that we also 
            //add a specific reference to the BODY property of this object as a second paramater.
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

    //this is the class for the POST BODY, which again can be found on Anthony's API documentation
    public class AddIngredientRequestBody
    {
        public string name;
        public string quantity;
    }
}
