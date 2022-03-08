using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingInquiry.Common
{
    public static class CommonFunction
    {
        public static string GetStringByKey(string key)
        {
            if (key == "DoorOpen")
                return "열림";
            switch (key)
            {
                case "DoorOpen":
                    return "열림";
                case "DoorClose":
                    return "닫힘";
                case "Acs_Fix":
                    return "열림 고정";
                case "GateOpenFixRelease":
                    return "열림 고정 해제";

                default:
                    break;
            }
            return string.Empty;
        }
    }
}
