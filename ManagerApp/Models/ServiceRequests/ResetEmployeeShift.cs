using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    public class ResetEmployeeShift : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Put;
        public List<ResetEmployeeShiftBody> Body { get; set; }

        public ResetEmployeeShift(string employeeid)
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/employees/" +  employeeid;

            Body = new List<ResetEmployeeShiftBody>();
            ResetEmployeeShiftBody resetEmployeeShiftBody = new ResetEmployeeShiftBody
            {
                propName = "current_shift",
                value = null
            };
            Body.Add(resetEmployeeShiftBody);
        }

        public async static Task<bool> SendResetEmployeeShift(string employeeid)
        {
            ResetEmployeeShift resetEmployeeShift = new ResetEmployeeShift(employeeid);
            var response = await ServiceRequestHandler.MakeServiceCall<ResetEmployeeResponse>(resetEmployeeShift, resetEmployeeShift.Body);
            if(response.nModified == 0)
            {
                return false;
            }
            else if(response.nModified == 1)
            {
                return true;
            }
            return false;
        }
    }

    public class ResetEmployeeResponse
    {
        public int n { get; set; }
        public int nModified { get; set; }
    }


    public class ResetEmployeeShiftBody
    {
        public string propName { get; set; }
        public string value { get; set; }
    }
}
