using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace ManagerApp.Models.ServiceRequests
{
    class GetOrdersRequest : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Get;

        public GetOrdersRequest()
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/Orders";
        }

        public static async Task<bool> SendGetOrdersRequest()
        {
            //make a new request object
            var serviceRequest = new GetOrdersRequest();
            //get a response
            var response = await ServiceRequestHandler.MakeServiceCall<OrderList>(serviceRequest);

            if (response == null)
            {
                //call failed
                return false;
            }
            else
            {
                // Add the response into the local database

                // Remove current contents
                RealmManager.RemoveAll<OrderList>();
                //RealmManager.RemoveAll<OrderItem>();

                // Assign each order item a unique ID
                Random rand = new Random();
                foreach (Order o in ((OrderList)response).orders.ToList()){

                    for (int i = 0; i < o.menuItems.Count(); ++i)
                    {
                        o.menuItems[i].newID = rand.Next(0, 1000000000).ToString();
                        while (RealmManager.Find<OrderItem>((o.menuItems[i].newID)) != null)
                        {
                            o.menuItems[i].newID = rand.Next(0, 1000000000).ToString();
                        }
                    }
                }
                RealmManager.AddOrUpdate<OrderList>(response);
                //call succeeded
                return true;
            }
        }
    }
}
