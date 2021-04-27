using MySqlConnector;
using System;

namespace SmartHomeAPI
{
    public class Appdb : IDisposable
    {
        public MySqlConnection Connection { get; }

        public Appdb(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
        }

        public void Dispose() => Connection.Dispose();
    }
}
