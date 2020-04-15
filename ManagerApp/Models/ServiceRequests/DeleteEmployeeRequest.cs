using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    public class DeleteEmployeeRequest : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Delete;

        public DeleteEmployeeRequest(string id)
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/Employees/" + id;
        }

        public async static Task<bool> SendDeleteEmployeeRequest(string id)
        {
            DeleteEmployeeRequest deleteEmployeeRequest = new DeleteEmployeeRequest(id);
            var response = await ServiceRequestHandler.MakeServiceCall<DeleteEmployeeRequestResponse>(deleteEmployeeRequest);

            if(response.message != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class DeleteEmployeeRequestResponse
    {
        public string message { get; set; } 
    }
}
