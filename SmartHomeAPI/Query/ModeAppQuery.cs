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
    public class ModeAppQuery
    {
        public Appdb Db { get; }

        public ModeAppQuery(Appdb db)
        {
            Db = db;
        }

        public async Task<ModeAppDTO> FindOneAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Mode_Lampu`, `Last_Change` FROM `tblmode_state` order by Last_Change desc limit 0,1";
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        private async Task<List<ModeAppDTO>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<ModeAppDTO>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new ModeAppDTO(Db)
                    {
                        Mode = reader.GetInt32(0),
                        Last_Change = reader.GetDateTime(1),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
