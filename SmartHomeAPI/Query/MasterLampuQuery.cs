using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;
using SmartHomeAPI.Models;

namespace SmartHomeAPI.Query
{
    public class MasterLampuQuery
    {
        public Appdb Db { get; }

        public MasterLampuQuery(Appdb db)
        {
            Db = db;
        }

        public async Task<MasterLampuDTO> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `Lampu_Name`, `Is_Active`, `Watt` FROM `mst_lampu` WHERE `Id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<MasterLampuDTO> CheckLastState(int id, int statePush)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"select aa.*, bb.lampu_state from mst_lampu aa join vi_last_state bb on aa.id = bb.lampu_id
                                    where bb.lampu_state = " + statePush + " and bb.lampu_id=" + id;
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<MasterLampuDTO>> AllDataSyn()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `Lampu_Name`, `Is_Active`, `Watt` FROM `mst_lampu` ORDER BY `Id`;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `mst_lampu`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<MasterLampuDTO>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<MasterLampuDTO>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new MasterLampuDTO(Db)
                    {
                        ID = reader.GetInt32(0),
                        Lampu_Name = reader.GetString(1),
                        Is_Active = reader.GetInt32(2),
                        Watt = reader.GetDecimal(3),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }

        //Update Biaya
        public async Task UpdateBiayaAsync(decimal decBiaya)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `mst_biayalistrik` SET `Nominal`= " + decBiaya + "  WHERE `Id` = 1;";
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
