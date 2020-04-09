using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    public class GetEmployeeListRequest : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Get;

        public GetEmployeeListRequest()
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/Employees";
        }

        public static async Task<bool> SendGetEmployeeListRequest()
        {
            var sendGetEmployeeListRequest = new GetEmployeeListRequest();
            var response = await ServiceRequestHandler.MakeServiceCall<EmployeeList>(sendGetEmployeeListRequest);

            if(response != null)
            {
                RealmManager.AddOrUpdate<EmployeeList>(response);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
