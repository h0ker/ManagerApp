using System.Collections.Generic;
using System.Net.Http;

namespace ManagerApp.Models
{
    public abstract class ServiceRequest
    {
        public abstract string Url { get; set; }
        public abstract HttpMethod Method { get; }
    }
}
