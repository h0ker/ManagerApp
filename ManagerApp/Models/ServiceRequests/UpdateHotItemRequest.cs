using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    class UpdateHotItemRequest : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Put;
        public IList<UpdateMenuItemRequestHotItem> Body { get; set; }

        public UpdateHotItemRequest(MenuItem tempItem)
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/menuitems/" + tempItem._id;
            Body = new List<UpdateMenuItemRequestHotItem>();

            UpdateMenuItemRequestHotItem UpdateMenuItemRequestHotItem;
            UpdateMenuItemRequestHotItem = new UpdateMenuItemRequestHotItem
            {
                propName = "category",
                value = tempItem.category
            };

            Body.Add(UpdateMenuItemRequestHotItem);

            UpdateMenuItemRequestHotItem = new UpdateMenuItemRequestHotItem
            {
                propName = "price",
                value = tempItem.price.ToString()
            };

            Body.Add(UpdateMenuItemRequestHotItem);

            UpdateMenuItemRequestHotItem = new UpdateMenuItemRequestHotItem
            {
                propName = "name",
                value = tempItem.name
            };

            Body.Add(UpdateMenuItemRequestHotItem);

            UpdateMenuItemRequestHotItem = new UpdateMenuItemRequestHotItem
            {
                propName = "nutrition",
                value = tempItem.nutrition
            };

            Body.Add(UpdateMenuItemRequestHotItem);

            UpdateMenuItemRequestHotItem = new UpdateMenuItemRequestHotItem
            {
                propName = "description",
                value = tempItem.description
            };

            Body.Add(UpdateMenuItemRequestHotItem);

            UpdateMenuItemRequestHotItem = new UpdateMenuItemRequestHotItem
            {
                propName = "isHot",
                value = tempItem.isHot.ToString().ToLower()
            };

            Body.Add(UpdateMenuItemRequestHotItem);
        }

        public async static Task<bool> SendUpdateMenuItemRequest(MenuItem menuItem)
        {
            UpdateHotItemRequest updateMenuItemRequestHotItem = new UpdateHotItemRequest(menuItem);
            var response = await ServiceRequestHandler.MakeServiceCall<DeleteResponse>(updateMenuItemRequestHotItem, updateMenuItemRequestHotItem.Body);
            if (response.electionId != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class UpdateMenuItemRequestHotItem
    {
        public string propName { get; set; }
        public string value { get; set; }
    }

}
