using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    public class UpdateEmployeeRequest : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Put;
        public List<UpdateEmployeeRequestBody> Body { get; set; }

        public UpdateEmployeeRequest(string id, string pay, string first_name, string last_name, string username, string position)
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/employees/" + id;

            Body = new List<UpdateEmployeeRequestBody>();

            UpdateEmployeeRequestBody updateEmployeeRequestBody = new UpdateEmployeeRequestBody
            {
                propName = "pay",
                value = pay
            };
            Body.Add(updateEmployeeRequestBody);

            updateEmployeeRequestBody = new UpdateEmployeeRequestBody
            {
                propName = "first_name",
                value = first_name
            };
            Body.Add(updateEmployeeRequestBody);

            updateEmployeeRequestBody = new UpdateEmployeeRequestBody
            {
                propName = "last_name",
                value = last_name
            };
            Body.Add(updateEmployeeRequestBody);
 
            updateEmployeeRequestBody = new UpdateEmployeeRequestBody
            {
                propName = "username",
                value = username
            };
            Body.Add(updateEmployeeRequestBody);
 
            updateEmployeeRequestBody = new UpdateEmployeeRequestBody
            {
                propName = "position",
                value = position
            };
            Body.Add(updateEmployeeRequestBody);
        }

        public async static Task<bool> SendUpdateEmployeeRequest(string id, string pay, string first_name, string last_name, string username, string position)
        {
            UpdateEmployeeRequest updateEmployeeRequest = new UpdateEmployeeRequest(id, pay, first_name, last_name, username, position);
            var response = await ServiceRequestHandler.MakeServiceCall<UpdateEmployeeResponse>(updateEmployeeRequest, updateEmployeeRequest.Body);
            if(response.nModified == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class UpdateEmployeeRequestBody
    {
        public string propName { get; set; }
        public string value { get; set; }
    }

    public class UpdateEmployeeResponse
    {
        public int nModified { get; set; }
    }
}
