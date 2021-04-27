using SmartHomeAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeAPI.Query
{
    public class GrafikQuery
    {
        public Appdb Db { get; }

        public GrafikQuery(Appdb db)
        {
            Db = db;
        }

        //Get Data Grafik in year
        public async Task<List<ListDataWattInYear>> GetGrafikWattInYear(int intYear)
        {
            string strQuery = "";

            for (int i = 1; i <= 12; i++)
            {
                //Get First Date
                var firstDayOfMonth = new DateTime(intYear, i, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                var firstDayOfMonthString = firstDayOfMonth.ToString("yyyy-MM-dd") + " 00:00:00";
                var lastDayOfMonthString = lastDayOfMonth.ToString("yyyy-MM-dd") + " 23:59:59";

                string strUnion = @" 
                                                UNION ALL 
                                            ";

                strQuery += @"select dt.month_str, dt.lampu_id, dt.lampu_name, sum(dt.watt * dt.menit) watt, on_date, off_date, sum(jam) jam, sum(menit) menit 
                                from (select " + i + @" month_str, aa.id lampu_id, aa.lampu_name, aa.watt, min(bb.on_date) on_date, max(bb.off_date) off_date, sum(ifnull(bb.jam, 0)) jam, sum(ifnull(bb.menit, 0)) menit 
                                from mst_lampu aa left join 
                                (select lampu_id, on_date, off_date, timestampdiff(MINUTE, on_date, off_date) AS menit, timestampdiff(HOUR, on_date, off_date) AS jam from (
                                select lampu_id, 
                                           case when on_date < '" + firstDayOfMonthString + @"' and ifnull(off_date, sysdate()) >= '" + firstDayOfMonthString + @"'
				                                then '" + firstDayOfMonthString + @"' else on_date end as on_date, 
		                                   case when ifnull(off_date, sysdate()) > '" + lastDayOfMonthString + @"' then '" + lastDayOfMonthString + @"' 
                                           else ifnull(off_date, sysdate()) end as off_date from vi_on_off_lamp 
                                           where case when on_date < '" + firstDayOfMonthString + @"' and ifnull(off_date, sysdate()) >= '" + firstDayOfMonthString + @"'
				                                then '" + firstDayOfMonthString + @"' else on_date end between '" + firstDayOfMonthString + @"' and '" + lastDayOfMonthString + @"') bb) bb on aa.id=bb.lampu_id 
                                                where aa.is_active=1 group by aa.id) dt ";

                strQuery += i == 12 ? " " : strUnion;
            }

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = strQuery;

            return await ReadAllAsync3(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<ListDataWattInYear>> ReadAllAsync3(DbDataReader reader)
        {
            var listData = new ListDataWattInYear();
            var posts = new List<ListDataWattInYear>();
            //listData.labels = new List<string>();
            List<int> lstDataset = new List<int>();
            //listData.datasets = new List<ListDataWattInYear.Dataset>();

            //Add list Data
            //string[] arrLabel = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            //listData.labels.AddRange(arrLabel);

            //Add Dataset
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    //decimal decWatt = Math.Round(reader.GetDecimal(3) * reader.GetInt32(7) / 60, 0);
                    decimal decWatt = Math.Round(reader.GetDecimal(3) / 60, 0);
                    lstDataset.Add(Convert.ToInt32(decWatt));
                }

                /*var post = new ListDataWattInYear.Dataset()
                {
                    data = lstDataset
                };

                listData.datasets.Add(post);*/

                listData.data = lstDataset.ToArray();
            }

            posts.Add(listData);

            return posts;
        }

        //GetTotalGrafikWattInYear
        public async Task<List<ListTotalDataWattInYear>> GetTotalGrafikWattInYear(int intYear)
        {
            string strQuery = "";

            for (int i = 1; i <= 12; i++)
            {
                //Get First Date
                var firstDayOfMonth = new DateTime(intYear, i, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                var firstDayOfMonthString = firstDayOfMonth.ToString("yyyy-MM-dd") + " 00:00:00";
                var lastDayOfMonthString = lastDayOfMonth.ToString("yyyy-MM-dd") + " 23:59:59";

                string strUnion = @" 
                                                UNION ALL 
                                            ";

                strQuery += @"select dt.month_str, dt.lampu_id, dt.lampu_name, sum(dt.watt * dt.menit) watt, on_date, off_date, sum(jam) jam, sum(menit) menit, ifnull((select nominal from mst_biayalistrik where id=1), 0) biaya 
                                from (select " + i + @" month_str, aa.id lampu_id, aa.lampu_name, aa.watt, min(bb.on_date) on_date, max(bb.off_date) off_date, sum(ifnull(bb.jam, 0)) jam, sum(ifnull(bb.menit, 0)) menit 
                                from mst_lampu aa left join 
                                (select lampu_id, on_date, off_date, timestampdiff(MINUTE, on_date, off_date) AS menit, timestampdiff(HOUR, on_date, off_date) AS jam from (
                                select lampu_id, 
                                           case when on_date < '" + firstDayOfMonthString + @"' and ifnull(off_date, sysdate()) >= '" + firstDayOfMonthString + @"'
				                                then '" + firstDayOfMonthString + @"' else on_date end as on_date, 
		                                   case when ifnull(off_date, sysdate()) > '" + lastDayOfMonthString + @"' then '" + lastDayOfMonthString + @"' 
                                           else ifnull(off_date, sysdate()) end as off_date from vi_on_off_lamp 
                                           where case when on_date < '" + firstDayOfMonthString + @"' and ifnull(off_date, sysdate()) >= '" + firstDayOfMonthString + @"'
				                                then '" + firstDayOfMonthString + @"' else on_date end between '" + firstDayOfMonthString + @"' and '" + lastDayOfMonthString + @"') bb) bb on aa.id=bb.lampu_id 
                                                where aa.is_active=1 group by aa.id) dt ";

                strQuery += i == 12 ? " " : strUnion;
            }

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = strQuery;

            return await ReadAllAsync4(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<ListTotalDataWattInYear>> ReadAllAsync4(DbDataReader reader)
        {
            var posts = new List<ListTotalDataWattInYear>();
            decimal decWatt = 0;
            decimal decNominalBiaya = 0;
            decimal decTotalBiaya = 0;

            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    decWatt += Math.Round(reader.GetDecimal(3) / 60, 0);
                    decNominalBiaya = reader.GetDecimal(8);
                }
            }

            var post = new ListTotalDataWattInYear(Db)
            {
                id = 1,
                Total_Nominal = decWatt.ToString("N0") + " W",
                Total_String = "Total Watt",
            };

            posts.Add(post);

            //Nominal
            decTotalBiaya = Math.Round(decWatt / 1000 * decNominalBiaya, 0);

            var post2 = new ListTotalDataWattInYear(Db)
            {
                id = 2,
                Total_Nominal = decTotalBiaya.ToString("N0"),
                Total_String = "Total (Rp)",
            };

            posts.Add(post2);

            return posts;
        }
    }
}
