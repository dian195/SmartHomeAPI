using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySqlConnector;

namespace SmartHomeAPI.Models.Filter
{
    public class MasterLampuInsertBody
    {
        public string Lampu_Name { get; set; }
        public int Is_Active { get; set; }
        public decimal Watt { get; set; }

        internal Appdb Db { get; set; }

        public MasterLampuInsertBody()
        {
        }

        internal MasterLampuInsertBody(Appdb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `mst_lampu` (`Lampu_Name`, `Is_Active`, `Watt`) VALUES (@Lampu_Name, @Is_Active, @Watt);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(int ID)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `mst_lampu` SET `Lampu_Name`= @Lampu_Name, `Is_Active` = @Is_Active, `Watt`= @Watt WHERE `Id` = " + ID + ";";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int ID)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `mst_lampu` WHERE `Id` = " + ID + ";";
            await cmd.ExecuteNonQueryAsync();
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
                Value = Is_Active,
            });
        }
    }
}
