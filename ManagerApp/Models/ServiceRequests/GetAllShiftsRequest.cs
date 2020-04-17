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

                ShiftList parsedList = new ShiftList();
                
                for(int i = 0; i<response.shifts.Count; i++)
                {
                    Shift interShift = response.shifts.ElementAt(i);
                    interShift.clock_in = interShift.clock_in.Substring(11, 8) + " " + interShift.clock_in.Substring(5,2) + "/" + interShift.clock_in.Substring(8,2) + "/" + interShift.clock_in.Substring(0,4);
                    if(!String.IsNullOrEmpty(interShift.clock_out))
                    {
                        interShift.clock_out = interShift.clock_out.Substring(11, 8) + " " + interShift.clock_out.Substring(5, 2) + "/" + interShift.clock_out.Substring(8, 2) + "/" + interShift.clock_out.Substring(0, 4); ;
                    }
                    parsedList.shifts.Add(interShift);
                }

                RealmManager.AddOrUpdate<ShiftList>(parsedList);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
