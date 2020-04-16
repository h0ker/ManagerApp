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

        public UpdateEmployeeRequest(string id, int position, double pay)
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/employees/" + id;

            Body = new List<UpdateEmployeeRequestBody>();

//            UpdateEmployeeRequestBody updateEmployeeRequestBody = new UpdateEmployeeRequestBody
 //           {
  //              propName = "position",
   //             value = position
    //        };
     //       Body.Add(updateEmployeeRequestBody);
            var updateEmployeeRequestBody = new UpdateEmployeeRequestBody
            {
                propName = "pay",
                value = pay.ToString()
            };
            Body.Add(updateEmployeeRequestBody);
        }

        public async static Task<bool> SendUpdateEmployeeRequest(string id, int position, double pay)
        {
            UpdateEmployeeRequest updateEmployeeRequest = new UpdateEmployeeRequest(id, position, pay);
            var response = await ServiceRequestHandler.MakeServiceCall<UpdateEmployeeRequestResponse>(updateEmployeeRequest, updateEmployeeRequest.Body);
            return true;
        }
    }

    public class UpdateEmployeeRequestResponse
    {
        public string message { get; set; }
    }

    public class UpdateEmployeeRequestBody
    {
        public string propName { get; set; }
        public string value { get; set; }
    }
}
