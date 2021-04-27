using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace SmartHomeAPI.Models
{
    public class HistoryStateLampuDTO
    {
        //public int ID { get; set; }
        public int Lampu_ID { get; set; }
        public string Lampu_Name { get; set; }
        public int Lampu_State { get; set; }
        public DateTime Change_Date { get; set; }

        internal Appdb Db { get; set; }

        public HistoryStateLampuDTO()
        {
        }

        internal HistoryStateLampuDTO(Appdb db)
        {
            Db = db;
        }

        //public async Task InsertAsync()
        //{
        //    using var cmd = Db.Connection.CreateCommand();
        //    cmd.CommandText = @"INSERT INTO `trx_history` (`Lampu_ID`, `Lampu_State`, `Change_Date`) VALUES (@Lampu_ID, @Lampu_State, @Change_Date)";
        //    BindParams(cmd);
        //    await cmd.ExecuteNonQueryAsync();
        //    ID = (int)cmd.LastInsertedId; //Autogeerated
        //}

        //public async Task UpdateAsync()
        //{
        //    using var cmd = Db.Connection.CreateCommand();
        //    cmd.CommandText = @"UPDATE `mst_lampu` SET `Lampu_Name`= @Lampu_Name, `Is_Active` = @Is_Active WHERE `Id` = @id;";
        //    BindParams(cmd);
        //    BindId(cmd);
        //    await cmd.ExecuteNonQueryAsync();
        //}

        //public async Task DeleteAsync()
        //{
        //    using var cmd = Db.Connection.CreateCommand();
        //    cmd.CommandText = @"DELETE FROM `mst_lampu` WHERE `Id` = @id;";
        //    BindId(cmd);
        //    await cmd.ExecuteNonQueryAsync();
        //}

        //private void BindId(MySqlCommand cmd)
        //{
        //    cmd.Parameters.Add(new MySqlParameter
        //    {
        //        ParameterName = "@id",
        //        DbType = DbType.Int64,
        //        Value = ID,
        //    });
        //}

        //private void BindParams(MySqlCommand cmd)
        //{
        //    cmd.Parameters.Add(new MySqlParameter
        //    {
        //        ParameterName = "@Lampu_ID",
        //        DbType = DbType.String,
        //        Value = Lampu_ID,
        //    });
        //    cmd.Parameters.Add(new MySqlParameter
        //    {
        //        ParameterName = "@Lampu_State",
        //        DbType = DbType.String,
        //        Value = Lampu_State,
        //    });
        //    cmd.Parameters.Add(new MySqlParameter
        //    {
        //        ParameterName = "@Change_Date",
        //        DbType = DbType.DateTime,
        //        Value = string.Format("{0:yyyy-MM-dd' 'HH':'mm':'ss}", Change_Date),
        //    });
        //}
    }

    public class ResponseHistoryStateLampu
    {
        public int status { get; set; }
        public string message { get; set; }
    }
}