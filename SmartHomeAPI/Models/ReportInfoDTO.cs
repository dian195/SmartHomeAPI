using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeAPI.Models
{
    public class ReportInfoDTO
    {
        public ReportInfoDTO()
        {
        }
    }

    public class ResponseReportInfo
    {
        public int status { get; set; }
        public string message { get; set; }

        internal Appdb Db { get; set; }

        public ResponseReportInfo()
        {
        }

        internal ResponseReportInfo(Appdb db)
        {
            Db = db;
        }
    }

    public class ListLampuDropDownReportInfo //Done
    {
        public string Label { get; set; }
        public string Value { get; set; }

        internal Appdb Db { get; set; }

        public ListLampuDropDownReportInfo()
        {
        }

        internal ListLampuDropDownReportInfo(Appdb db)
        {
            Db = db;
        }
    }

    public class listDataLampuByWattReportInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public string total_watt { get; set; }

        internal Appdb Db { get; set; }

        public listDataLampuByWattReportInfo()
        {
        }

        internal listDataLampuByWattReportInfo(Appdb db)
        {
            Db = db;
        }
    }

    public class listTotalByWatt
    {
        public int id { get; set; }
        public string Total_String { get; set; }
        public string Total_Nominal { get; set; }

        internal Appdb Db { get; set; }

        public listTotalByWatt()
        {
        }

        internal listTotalByWatt(Appdb db)
        {
            Db = db;
        }
    }

    public class listDataLampuByWaktu
    {
        public int id { get; set; }
        public string name { get; set; }
        public string waktu { get; set; }

        internal Appdb Db { get; set; }

        public listDataLampuByWaktu()
        {
        }

        internal listDataLampuByWaktu(Appdb db)
        {
            Db = db;
        }
    }

    public class listTotalByWaktu
    {
        public int id { get; set; }
        public string Total_String { get; set; }
        public string Total_Nominal { get; set; }

        internal Appdb Db { get; set; }

        public listTotalByWaktu()
        {
        }

        internal listTotalByWaktu(Appdb db)
        {
            Db = db;
        }
    }
}
