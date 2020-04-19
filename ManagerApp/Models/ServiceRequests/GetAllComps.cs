using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models.ServiceRequests
{
    public class GetAllComps : ServiceRequest
    {
        public override string Url { get; set; }
        public override HttpMethod Method => HttpMethod.Get;

        public GetAllComps(string id)
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/comps/" + id;
        }

        public GetAllComps()
        {
            Url = "https://dijkstras-steakhouse-restapi.herokuapp.com/comps";
        }

        public async static Task<bool> SendAllCompsNoArgs()
        {
            GetAllComps getAllComps = new GetAllComps();
            var response = await ServiceRequestHandler.MakeServiceCall<Comps>(getAllComps);
            if(response != null)
            {
                RealmManager.RemoveAll<Comp>();
                RealmManager.RemoveAll<Comps>();
                RealmManager.AddOrUpdate<Comps>(response);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async static Task<bool> SendGetAllCompsEmployee(string id)
        {
            GetAllComps getAllComps = new GetAllComps(id);
            var response = await ServiceRequestHandler.MakeServiceCall<CompList>(getAllComps);
            if(response != null)
            {
                RealmManager.RemoveAll<CompList>();
                RealmManager.RemoveAll<CompList>();
                RealmManager.AddOrUpdate<CompList>(response);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
