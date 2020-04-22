using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models
{
    public class ClockOut : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Put;
        public List<ClockOutBody> Body { get; set; }

        public ClockOut(string shiftid)
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/shifts/" +  shiftid;

            Body = new List<ClockOutBody>();
            ClockOutBody clockOutBody = new ClockOutBody
            {
                propName = "clock_out",
                value = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            };
            Body.Add(clockOutBody);
        }

        public async static Task<bool> SendClockOutRequest(string shiftid)
        {
            ClockOut clockOut = new ClockOut(shiftid);
            var response = await ServiceRequestHandler.MakeServiceCall<ClockOutResponse>(clockOut, clockOut.Body);
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

    public class ClockOutResponse
    {
        public int n { get; set; }
        public int nModified { get; set; }
    }

    public class ClockOutBody
    {
        public string propName { get; set; }
        public string value { get; set; }
    }
}
