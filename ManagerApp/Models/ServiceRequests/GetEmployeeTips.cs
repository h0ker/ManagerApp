using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    public class GetEmployeeTips : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Get;

        public GetEmployeeTips(string id)
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/tips/" + id;
        }

        public async static Task<bool> SendGetEmployeeTips(string id)
        {
            GetEmployeeTips getEmployeeTips = new GetEmployeeTips(id);
            var response = await ServiceRequestHandler.MakeServiceCall<Tips>(getEmployeeTips);
            if(response != null)
            {
                RealmManager.RemoveAll<Tips>();
                RealmManager.RemoveAll<Tip>();
                RealmManager.AddOrUpdate<Tips>(response);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
