using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeAPI.Models.Filter
{
    public class HistoryFilterBody
    {
        public int Lampu_ID { get; set; }
        public int Lampu_State { get; set; }
        public DateTime Date_From { get; set; }
        public DateTime Date_To { get; set; }
    }
}
