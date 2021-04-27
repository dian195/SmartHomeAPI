using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;
using SmartHomeAPI.Models;

namespace SmartHomeAPI.Query
{
    public class ActionLampuQuery
    {
        public Appdb Db { get; }

        public ActionLampuQuery(Appdb db)
        {
            Db = db;
        }

        public async Task<ActionLampuDTO> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Lampu_ID`, `Lampu_State`, `Change_Date` FROM `trx_history` WHERE `Id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        private async Task<List<ActionLampuDTO>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<ActionLampuDTO>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new ActionLampuDTO(Db)
                    {
                        ID = reader.GetInt32(0),
                        Lampu_State = reader.GetInt32(1),
                        Change_Date = reader.GetDateTime(2),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}