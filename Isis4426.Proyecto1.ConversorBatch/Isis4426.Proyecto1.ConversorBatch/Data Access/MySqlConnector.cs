using Isis4426.Proyecto1.ConversorBatch.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isis4426.Proyecto1.ConversorBatch.Data_Access
{
    abstract class MySqlConnector
    {
        private string serverConnectionString;
        protected MySqlConnection Connection;

        public MySqlConnector()
        {
            serverConnectionString = string.Format("server={0};user id={1};password={2};port={3};database={4};",
                Configuration.Instance.Database.Server,
                Configuration.Instance.Database.Username,
                Configuration.Instance.Database.Password,
                Configuration.Instance.Database.Port,
                Configuration.Instance.Database.Schema);
        }

        protected MySqlConnection GetConnection()
        {
            if (Connection != null)
            {
                Connection.Close();
                Connection.Dispose();
                Connection = null;
            }

            Connection = new MySqlConnection(serverConnectionString);
            Connection.Open();

            return Connection;
        }

        protected MySqlCommand GetCommand(string sqlQuery, List<MySqlParameter> parameters = null)
        {
            MySqlCommand command = new MySqlCommand(sqlQuery);
            command.CommandType = System.Data.CommandType.Text;
            if (parameters != null)
            {
                foreach (MySqlParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }

        internal abstract object CreateObject(MySqlDataReader dr);
        internal abstract List<MySqlParameter> ResortObject(object instanciaClase);
        internal abstract void TestConnection();
    }
}
