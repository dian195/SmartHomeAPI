using MySqlConnector;
using SmartHomeAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeAPI.Query
{
    public class HistoryStateLampuQuery
    {
        public Appdb Db { get; }

        public HistoryStateLampuQuery(Appdb db)
        {
            Db = db;
        }

        public async Task<HistoryStateLampuDTO> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Lampu_ID`, `Lampu_Name`, `Lampu_State`, `Change_Date` FROM `vi_last_state` WHERE `Lampu_ID`= @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<HistoryStateLampuDTO>> AllDataSyn()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Lampu_ID`, `Lampu_Name`, `Lampu_State`, `Change_Date` FROM `vi_last_state` ORDER BY `Lampu_ID`;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task<List<HistoryStateLampuDTO>> SelectWithFilterSync(int Lampu_ID, int Lampu_State, DateTime Date_From, DateTime Date_To)
        {
            string strWhere = "1=1";
            strWhere += Lampu_ID == 0 ? "" : " and Lampu_ID = " + Lampu_ID;
            strWhere += Lampu_State == 0 ? "" : " and Lampu_State = " + Lampu_State;

            if (Date_From != null && Date_To != null)
            {
                if (Date_From.ToString("dd/MM/yyyy") != "01/01/0001" && Date_To.ToString("dd/MM/yyyy") != "01/01/0001")
                {
                    strWhere += " and DATE_FORMAT(Change_Date, '%Y-%m-%d') between '" + Date_From.ToString("yyyy-MM-dd") + "' and '" + Date_To.ToString("yyyy-MM-dd") + "'";
                }
            }

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"Select aa.Lampu_ID, bb.Lampu_Name, aa.Lampu_State, aa.Change_Date from trx_history aa join mst_lampu bb on aa.lampu_id=bb.id where " + strWhere;
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<HistoryStateLampuDTO>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<HistoryStateLampuDTO>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new HistoryStateLampuDTO(Db)
                    {
                        Lampu_ID = reader.GetInt32(0),
                        Lampu_Name = reader.GetString(1),
                        Lampu_State = reader.GetInt32(2),
                        Change_Date = reader.GetDateTime(3),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
