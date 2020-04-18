using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    public class GetMenuItemsRequest : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Get;

        public GetMenuItemsRequest()
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/menuItems";
        }

        public async static Task<bool> SendGetMenuItemsRequest()
        {
            GetMenuItemsRequest getMenuItemsRequest = new GetMenuItemsRequest();
            var response = await ServiceRequestHandler.MakeServiceCall<MenuItemList>(getMenuItemsRequest);

            if(response.menuItems != null)
            {
                RealmManager.RemoveAll<MenuItemList>();
                RealmManager.AddOrUpdate<MenuItemList>(response);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
