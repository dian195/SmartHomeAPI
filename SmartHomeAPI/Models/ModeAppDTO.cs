using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeAPI.Models
{
    public class ModeAppDTO
    {
        public int Mode { get; set; }
        public DateTime Last_Change { get; set; }
        
        internal Appdb Db { get; set; }

        public ModeAppDTO()
        {
        }

        internal ModeAppDTO(Appdb db)
        {
            Db = db;
        }

        public async Task UpdateInsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `tblmode_state` SET `Mode_Lampu`= @Mode_Lampu, `Last_Change` = @Last_Change;
                                INSERT INTO `tblhistory_mode` (`Mode_Lampu`, `Change_Date`) VALUES (@Mode_Lampu, @Last_Change)";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Mode_Lampu",
                DbType = DbType.Int32,
                Value = Mode,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Last_Change",
                DbType = DbType.DateTime,
                Value = string.Format("{0:yyyy-MM-dd' 'HH':'mm':'ss}", Last_Change),
            });
            await cmd.ExecuteNonQueryAsync();
        }
    }

    public class ResponseModeApp
    {
        public int status { get; set; }
        public string message { get; set; }
    }
}