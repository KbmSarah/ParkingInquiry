using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingInquiry.Common
{
    public class ParkingSession : RestClient, IDisposable
    {

        private const int DefaultTimeout = 10000;
        public ParkingSession(string url) : base(url) { }

        public async Task<string> RequestApi(string url, Method method, string requestParam)
        {
            string requestUrl = "client/sale-stt";
            RestRequest request = new RestRequest(requestUrl, Method.POST);

            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", requestParam, ParameterType.RequestBody);

            IRestResponse response = await ExecuteAsync(request);

            return response.Content;
        }

        
        public void Dispose()
        {
        }
    }
}
