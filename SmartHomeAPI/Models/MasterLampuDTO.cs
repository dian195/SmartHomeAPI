using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySqlConnector;

namespace SmartHomeAPI.Models
{
    public class MasterLampuDTO
    {
        public int ID { get; set; }
        public string Lampu_Name { get; set; }
        public int Is_Active { get; set; }
        public decimal Watt { get; set; }

        internal Appdb Db { get; set; }

        public MasterLampuDTO()
        {
        }

        internal MasterLampuDTO(Appdb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `mst_lampu` (`Lampu_Name`, `Is_Active`, `Watt`) VALUES (@Lampu_Name, @Is_Active, @Watt);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            ID = (int)cmd.LastInsertedId; //Autogeerated
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `mst_lampu` SET `Lampu_Name`= @Lampu_Name, `Is_Active` = @Is_Active, `Watt` = @Watt WHERE `Id` = @id;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `mst_lampu` WHERE `Id` = @id;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int64,
                Value = ID,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Lampu_Name",
                DbType = DbType.String,
                Value = Lampu_Name,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Is_Active",
                DbType = DbType.String,
                Value = Is_Active,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Watt",
                DbType = DbType.Decimal,
                Value = Watt,
            });
        }
    }

    public class ResponseMasterLampu
    {
        public int status { get; set; }
        public string message { get; set; }
    }
}
