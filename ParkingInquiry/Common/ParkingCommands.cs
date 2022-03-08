using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParkingInquiry.Common
{
    public static class ParkingCommands
    {
        private static ParkingSession Session = null;
        private static string loginUrl = null;

        public static void SessionStart(string address, int port)
        {
            if (Session != null)
                Session.Dispose();

            loginUrl = $"http://{address}:{port}/";
            Session = new ParkingSession(loginUrl);
            //Console.WriteLine(MethodBase.GetCurrentMethod().Name);
        }

        public static ParkingReceive RequestSales(ParkingRequest RequestParameter)
        {
            string param = JsonConvert.SerializeObject(RequestParameter);
            string result = null;

            Task.Factory.StartNew(() => 
            {
                result = Session?.RequestApi(loginUrl, RestSharp.Method.POST, param).Result;
            }).Wait();

            return result != null ? JsonConvert.DeserializeObject<ParkingReceive>(result) : null;
        }
    }
}
