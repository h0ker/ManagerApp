using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    public class AddEmployeeRequest : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Post;
        public AddEmployeeRequestBody Body { get; set; }

        public AddEmployeeRequest(string fName, string lName, string userName, string passWord, int position, double pay)
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/Employees";

            Body = new AddEmployeeRequestBody
            {
                first_name = fName,
                last_name = lName,
                username = userName,
                password = passWord,
                position = position,
                pay = pay
            };
        }

        public async static Task<bool> SendAddEmployeeRequest(string fName, string lName, string userName, string passWord, int position, double pay)
        {
            AddEmployeeRequest addEmployeeRequest = new AddEmployeeRequest(fName, lName, userName, passWord, position, pay);

            var response = await ServiceRequestHandler.MakeServiceCall<AddEmployeeRequestResponse>(addEmployeeRequest, addEmployeeRequest.Body);

            if(response.employee != null)
            {
                return true; 
            }
            else
            {
                return false;
            }
        }
    }

    public class AddEmployeeRequestBody
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int position { get; set; }
        public double pay { get; set; }
    }

    public class AddEmployeeRequestResponse
    {
        public string message { get; set; }
        public Employee employee { get; set; }
    }
}
