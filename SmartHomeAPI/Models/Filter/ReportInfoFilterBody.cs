using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeAPI.Models.Filter
{
    public class ReportInfoFilterBody
    {

    }

    public class InfoWaktuLampuHidupFilter
    {
        public int Lampu_ID { get; set; }
        public DateTime Date_From { get; set; }
        public DateTime Date_To { get; set; }
    }
}
