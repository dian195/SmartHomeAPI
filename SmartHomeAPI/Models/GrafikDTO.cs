using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeAPI.Models
{
    public class GrafikDTO
    {
        public GrafikDTO()
        {
        }
    }

    public class ResponseGrafik
    {
        public int status { get; set; }
        public string message { get; set; }

        internal Appdb Db { get; set; }

        public ResponseGrafik()
        {
        }

        internal ResponseGrafik(Appdb db)
        {
            Db = db;
        }
    }

    public class ListDataWattInYear
    {
        internal Appdb Db { get; set; }

        public ListDataWattInYear()
        {
        }

        internal ListDataWattInYear(Appdb db)
        {
            Db = db;
        }

        //public List<string> labels { get; set; }
        //public List<Dataset> datasets { get; set; }
        public Array data { get; set; }

        /*public class Dataset
        {
            public List<int> data { get; set; }
        }*/
    }

    //Total
    public class ListTotalDataWattInYear
    {
        internal Appdb Db { get; set; }
        
        public ListTotalDataWattInYear()
        {
        }

        internal ListTotalDataWattInYear(Appdb db)
        {
            Db = db;
        }

        public int id { get; set; }
        public string Total_String { get; set; }
        public string Total_Nominal { get; set; }
    }
}
