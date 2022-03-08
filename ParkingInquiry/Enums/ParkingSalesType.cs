using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingInquiry.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ParkingSalesType
    {
        [Description("일반")]
        Normal,
        [Description("정기")]
        Season
    }
}
