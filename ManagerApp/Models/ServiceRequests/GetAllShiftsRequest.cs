using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    public class GetAllShiftsRequest : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Get;

        public GetAllShiftsRequest(string employeeid)
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/shifts/" + employeeid;
        }

        public async static Task<bool> SendGetAllShiftsRequest(string employeeid)
        {
            GetAllShiftsRequest getAllShiftsRequest = new GetAllShiftsRequest(employeeid);
            var response = await ServiceRequestHandler.MakeServiceCall<ShiftList>(getAllShiftsRequest);
            if(response != null)
            {
                RealmManager.RemoveAll<ShiftList>();
                RealmManager.AddOrUpdate<ShiftList>(response);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
