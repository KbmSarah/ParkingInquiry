using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingInquiry.Common
{
    public class ParkingRequest
    {
        public string dt_from { get; set; }
        public string dt_to { get; set; }
        public string stt_type { get; set; } // 월별통계 : month, 일별통계 : day
        public int page { get; set; }
        public int row_count { get; set; }
        public int park_id { get; set; }
    }

    public class ParkingReceive : ParkingApiResultBase
    {
        public int total_count { get; set; }
        public List<ParkingItem> sale_list { get; set; }
    }

    public class ParkingItem
    {
        public string date { get; set; }
        public ParkingResult normal { get; set; }
        public ParkingResult commuter { get; set; }
        public Int64 dc_count { get; set; }
        public Int64 dc { get; set; }
        public Int64 web_dc_count { get; set; }
        public Int64 web_dc { get; set; }
    }

    public class ParkingResult
    {
        public Int64 cash_count { get; set; }
        public Int64 cash { get; set; }
        public Int64 credit_count { get; set; }
        public Int64 credit { get; set; }
        public Int64 trns_count { get; set; }
        public Int64 trns { get; set; }
        public Int64 etc_count { get; set; }
        public Int64 etc { get; set; }
    }

    public class ParkingApiResultBase
    {
        public string result_code { get; set; }
        public string result_msg { get; set; }
    }
}
