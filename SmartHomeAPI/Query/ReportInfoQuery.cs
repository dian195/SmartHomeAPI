using SmartHomeAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeAPI.Query
{
    public class ReportInfoQuery
    {
        public Appdb Db { get; }

        public ReportInfoQuery(Appdb db)
        {
            Db = db;
        }

        //Get Data Lampu For DropDown
        public async Task<List<ListLampuDropDownReportInfo>> GetDataLampuForDDLInfo()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"select 0 ID, 'Semua Lampu' Lampu_Name from dual
                                    union all
                                select ID, Lampu_Name from mst_lampu where is_active=1 order by ID";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<ListLampuDropDownReportInfo>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<ListLampuDropDownReportInfo>();

            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new ListLampuDropDownReportInfo(Db)
                    {
                        Value = reader.GetInt32(0).ToString(),
                        Label = reader.GetString(1),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }

        //Get Data Report Info Lampu By Watt
        public async Task<listDataLampuByWattReportInfo> GetReportInfoLampuByWatt()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"select 0 ID, 'Semua Lampu' Lampu_Name from dual
                                    union all
                                select ID, Lampu_Name from mst_lampu where is_active=1 order by ID";
            var result = await ReadAllAsync2(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        private async Task<List<listDataLampuByWattReportInfo>> ReadAllAsync2(DbDataReader reader)
        {
            var posts = new List<listDataLampuByWattReportInfo>();

            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new listDataLampuByWattReportInfo(Db)
                    {
                        id = reader.GetInt32(0).ToString(),
                        name = reader.GetString(1),
                        total_watt = reader.GetString(2),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }

        //Get Data Report Info Lampu By Total Waktu
        public async Task<List<listDataLampuByWaktu>> GetReportInfoLampuByWaktuHidup(DateTime firstDayOfMonthString, DateTime lastDayOfMonthString, int intLampu)
        {
            //DateTime date = DateTime.Now;
            //var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            //var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            //var firstDayOfMonthString = firstDayOfMonth.ToString("yyyy-MM-dd") + " 00:00:00";
            //var lastDayOfMonthString = lastDayOfMonth.ToString("yyyy-MM-dd") + " 23:59:59";

            string strWhereLampu = "1=1";
            strWhereLampu = intLampu == 0 ? strWhereLampu : "aa.id=" + intLampu;
            using var cmd = Db.Connection.CreateCommand();
            /*cmd.CommandText = @"select aa.id lampu_id, aa.lampu_name, aa.watt, bb.on_date, bb.off_date, ifnull(bb.jam, 0) jam, ifnull(bb.menit, 0) menit from mst_lampu aa left join 
                                (select lampu_id, on_date, off_date, timestampdiff(MINUTE, on_date, off_date) AS menit, timestampdiff(HOUR, on_date, off_date) AS jam from (
                                select lampu_id, 
                                           case when on_date < @FROM_DATE and ifnull(off_date, sysdate()) >= @FROM_DATE
				                                then @FROM_DATE else on_date end as on_date, 
		                                   case when ifnull(off_date, sysdate()) > @TO_DATE then @TO_DATE 
                                           else ifnull(off_date, sysdate()) end as off_date from vi_on_off_lamp 
                                           where case when on_date < @FROM_DATE and ifnull(off_date, sysdate()) >= @FROM_DATE
				                                then @FROM_DATE else on_date end between @FROM_DATE and @TO_DATE) bb) bb on aa.id=bb.lampu_id 
                                                where aa.is_active=1 and " + strWhereLampu + " order by aa.id";*/

            cmd.CommandText = @"select aa.id lampu_id, aa.lampu_name, aa.watt, min(bb.on_date) on_date, max(bb.off_date) off_date, sum(ifnull(bb.jam, 0)) jam, sum(ifnull(bb.menit, 0)) menit from mst_lampu aa left join 
                                (select lampu_id, on_date, off_date, timestampdiff(MINUTE, on_date, off_date) AS menit, timestampdiff(HOUR, on_date, off_date) AS jam from (
                                select lampu_id, 
                                           case when on_date < @FROM_DATE and ifnull(off_date, sysdate()) >= @FROM_DATE
				                                then @FROM_DATE else on_date end as on_date, 
		                                   case when ifnull(off_date, sysdate()) > @TO_DATE then @TO_DATE 
                                           else ifnull(off_date, sysdate()) end as off_date from vi_on_off_lamp 
                                           where case when on_date < @FROM_DATE and ifnull(off_date, sysdate()) >= @FROM_DATE
				                                then @FROM_DATE else on_date end between @FROM_DATE and @TO_DATE) bb) bb on aa.id=bb.lampu_id 
                                                where aa.is_active=1 and " + strWhereLampu + " group by lampu_id order by aa.id";

            cmd.Parameters.AddWithValue("@FROM_DATE", firstDayOfMonthString.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@TO_DATE", lastDayOfMonthString.ToString("yyyy-MM-dd HH:mm:ss"));

            return await ReadAllAsync3(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<listDataLampuByWaktu>> ReadAllAsync3(DbDataReader reader)
        {
            var posts = new List<listDataLampuByWaktu>();

            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    int hours = (reader.GetInt32(6) - reader.GetInt32(6) % 60) / 60;
                    string strWaktu = hours + " Jam " + (reader.GetInt32(6) - hours * 60) + " Menit";

                    var post = new listDataLampuByWaktu(Db)
                    {
                        id = reader.GetInt32(0),
                        name = reader.GetString(1),
                        waktu = strWaktu,
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }

        //Total Waktu Hidup
        public async Task<List<listTotalByWaktu>> InfoTotalWaktuLampuHidup(DateTime firstDayOfMonthString, DateTime lastDayOfMonthString, int intLampu)
        {
            //DateTime date = DateTime.Now;
            //var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            //var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            //var firstDayOfMonthString = firstDayOfMonth.ToString("yyyy-MM-dd") + " 00:00:00";
            //var lastDayOfMonthString = lastDayOfMonth.ToString("yyyy-MM-dd") + " 23:59:59";

            string strWhereLampu = "1=1";
            strWhereLampu = intLampu == 0 ? strWhereLampu : "aa.id=" + intLampu;
            using var cmd = Db.Connection.CreateCommand();

            cmd.CommandText = @"select aa.id lampu_id, sum(ifnull(bb.jam, 0)) jam, sum(ifnull(bb.menit, 0)) menit from mst_lampu aa left join 
                                (select lampu_id, on_date, off_date, timestampdiff(MINUTE, on_date, off_date) AS menit, timestampdiff(HOUR, on_date, off_date) AS jam from (
                                select lampu_id, 
                                           case when on_date < @FROM_DATE and ifnull(off_date, sysdate()) >= @FROM_DATE
				                                then @FROM_DATE else on_date end as on_date, 
		                                   case when ifnull(off_date, sysdate()) > @TO_DATE then @TO_DATE 
                                           else ifnull(off_date, sysdate()) end as off_date from vi_on_off_lamp 
                                           where case when on_date < @FROM_DATE and ifnull(off_date, sysdate()) >= @FROM_DATE
				                                then @FROM_DATE else on_date end between @FROM_DATE and @TO_DATE) bb) bb on aa.id=bb.lampu_id 
                                                where aa.is_active=1 and " + strWhereLampu + " ";

            cmd.Parameters.AddWithValue("@FROM_DATE", firstDayOfMonthString.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@TO_DATE", lastDayOfMonthString.ToString("yyyy-MM-dd HH:mm:ss"));

            return await ReadAllAsync4(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<listTotalByWaktu>> ReadAllAsync4(DbDataReader reader)
        {
            var posts = new List<listTotalByWaktu>();

            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    int hours = (reader.GetInt32(2) - reader.GetInt32(2) % 60) / 60;
                    string strWaktu = hours + " Jam " + (reader.GetInt32(2) - hours * 60) + " Menit";

                    var post = new listTotalByWaktu(Db)
                    {
                        id = reader.GetInt32(0),
                        Total_Nominal = strWaktu,
                        Total_String = "Total",
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }

        //Get Data Report Info Lampu By Total Watt
        public async Task<List<listDataLampuByWattReportInfo>> GetInfoWattLampuHidup(DateTime firstDayOfMonthString, DateTime lastDayOfMonthString, int intLampu)
        {
            //DateTime date = DateTime.Now;
            //var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            //var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            //var firstDayOfMonthString = firstDayOfMonth.ToString("yyyy-MM-dd") + " 00:00:00";
            //var lastDayOfMonthString = lastDayOfMonth.ToString("yyyy-MM-dd") + " 23:59:59";

            string strWhereLampu = "1=1";
            strWhereLampu = intLampu == 0 ? strWhereLampu : "aa.id=" + intLampu;
            using var cmd = Db.Connection.CreateCommand();

            cmd.CommandText = @"select aa.id lampu_id, aa.lampu_name, ifnull(aa.watt, 0) watt, min(bb.on_date) on_date, max(bb.off_date) off_date, sum(ifnull(bb.jam, 0)) jam, sum(ifnull(bb.menit, 0)) menit from mst_lampu aa left join 
                                (select lampu_id, on_date, off_date, timestampdiff(MINUTE, on_date, off_date) AS menit, timestampdiff(HOUR, on_date, off_date) AS jam from (
                                select lampu_id, 
                                           case when on_date < @FROM_DATE and ifnull(off_date, sysdate()) >= @FROM_DATE
				                                then @FROM_DATE else on_date end as on_date, 
		                                   case when ifnull(off_date, sysdate()) > @TO_DATE then @TO_DATE 
                                           else ifnull(off_date, sysdate()) end as off_date from vi_on_off_lamp 
                                           where case when on_date < @FROM_DATE and ifnull(off_date, sysdate()) >= @FROM_DATE
				                                then @FROM_DATE else on_date end between @FROM_DATE and @TO_DATE) bb) bb on aa.id=bb.lampu_id 
                                                where aa.is_active=1 and " + strWhereLampu + " group by lampu_id order by aa.id";

            cmd.Parameters.AddWithValue("@FROM_DATE", firstDayOfMonthString.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@TO_DATE", lastDayOfMonthString.ToString("yyyy-MM-dd HH:mm:ss"));

            return await ReadAllAsync5(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<listDataLampuByWattReportInfo>> ReadAllAsync5(DbDataReader reader)
        {
            var posts = new List<listDataLampuByWattReportInfo>();

            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    //int hours = (reader.GetInt32(6) - reader.GetInt32(6) % 60) / 60;
                    //string strWatt = hours + " Jam " + (reader.GetInt32(6) - hours * 60) + " Menit";
                    string strWatt = Math.Round(reader.GetDecimal(2) * reader.GetInt32(6) / 60, 0).ToString("N0") + " W";

                    var post = new listDataLampuByWattReportInfo(Db)
                    {
                        id = reader.GetInt32(0).ToString(),
                        name = reader.GetString(1),
                        total_watt = strWatt,
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }

        //Total Watt
        public async Task<List<listTotalByWatt>> InfoTotalWattLampuHidup(DateTime firstDayOfMonthString, DateTime lastDayOfMonthString, int intLampu)
        {
            //DateTime date = DateTime.Now;
            //var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            //var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            //var firstDayOfMonthString = firstDayOfMonth.ToString("yyyy-MM-dd") + " 00:00:00";
            //var lastDayOfMonthString = lastDayOfMonth.ToString("yyyy-MM-dd") + " 23:59:59";

            string strWhereLampu = "1=1";
            strWhereLampu = intLampu == 0 ? strWhereLampu : "aa.id=" + intLampu;
            using var cmd = Db.Connection.CreateCommand();

            cmd.CommandText = @"select dt.lampu_id, sum(dt.watt * dt.menit / 60) watt, sum(jam) jam, sum(menit) menit, biaya from ( 
                                select aa.id lampu_id, ifnull(aa.watt, 0) watt, sum(ifnull(bb.jam, 0)) jam, sum(ifnull(bb.menit, 0)) menit, (select nominal from mst_biayalistrik where id=1) biaya from mst_lampu aa left join 
                                (select lampu_id, on_date, off_date, timestampdiff(MINUTE, on_date, off_date) AS menit, timestampdiff(HOUR, on_date, off_date) AS jam from (
                                select lampu_id, 
                                           case when on_date < @FROM_DATE and ifnull(off_date, sysdate()) >= @FROM_DATE
				                                then @FROM_DATE else on_date end as on_date, 
		                                   case when ifnull(off_date, sysdate()) > @TO_DATE then @TO_DATE 
                                           else ifnull(off_date, sysdate()) end as off_date from vi_on_off_lamp 
                                           where case when on_date < @FROM_DATE and ifnull(off_date, sysdate()) >= @FROM_DATE
				                                then @FROM_DATE else on_date end between @FROM_DATE and @TO_DATE) bb) bb on aa.id=bb.lampu_id 
                                                where aa.is_active=1 and " + strWhereLampu + " group by aa.id) dt";

            cmd.Parameters.AddWithValue("@FROM_DATE", firstDayOfMonthString.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@TO_DATE", lastDayOfMonthString.ToString("yyyy-MM-dd HH:mm:ss"));

            return await ReadAllAsync6(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<listTotalByWatt>> ReadAllAsync6(DbDataReader reader)
        {
            var posts = new List<listTotalByWatt>();
            decimal decTotalWatt = 0;
            decimal decBiaya = 0;
            decimal decNominal = 0;

            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    //decTotalWatt += Math.Round(reader.GetDecimal(1) * reader.GetInt32(3) / 60, 0);
                    decTotalWatt += Math.Round(reader.GetDecimal(1), 0);
                    decBiaya = reader.GetDecimal(4);
                }

                string strWatt = decTotalWatt.ToString("N0") + " W";

                var post = new listTotalByWatt(Db)
                {
                    id = 1,
                    Total_Nominal = strWatt,
                    Total_String = "Total",
                };

                posts.Add(post);
            }

            //Nominal
            decNominal = Math.Round(decTotalWatt / 1000 * decBiaya, 0);

            var post2 = new listTotalByWatt(Db)
            {
                id = 0,
                Total_Nominal = decNominal.ToString("N2"),
                Total_String = "Rp.",
            };

            posts.Add(post2);

            return posts;
        }
    }
}
