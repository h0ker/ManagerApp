using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    public class ClockIn : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Post;
        public ClockInBody Body { get; set; }

        public ClockIn(string employeeid)
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/shifts";

            Body = new ClockInBody
            {
                employee_id = employeeid,
                clock_in = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            };
        }

        public async static Task<bool> SendClockInRequest(string employeeid)
        {
            ClockIn clockIn = new ClockIn(employeeid);
            var response = await ServiceRequestHandler.MakeServiceCall<ClockInRespoonse>(clockIn, clockIn.Body);
            if(response.shift_id != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class ClockInRespoonse
    {
        public string message { get; set; }
        public string shift_id { get; set; }
    }

    public class ClockInBody
    {
        public string employee_id { get; set; }
        public string clock_in { get; set; }
    }
}
