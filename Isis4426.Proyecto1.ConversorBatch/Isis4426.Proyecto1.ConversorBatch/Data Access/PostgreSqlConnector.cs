using Isis4426.Proyecto1.ConversorBatch.Models;
using Npgsql;
using System.Collections.Generic;

namespace Isis4426.Proyecto1.ConversorBatch.Data_Access
{
    abstract class PostgreSqlConnector
    {
        private string serverConnectionString;
        protected NpgsqlConnection Connection;

        public PostgreSqlConnector()
        {
            serverConnectionString = string.Format("server={0};user id={1};password={2};port={3};database={4};",
                Configuration.Instance.Database.Server,
                Configuration.Instance.Database.Username,
                Configuration.Instance.Database.Password,
                Configuration.Instance.Database.Port,
                Configuration.Instance.Database.Schema);
        }

        protected NpgsqlConnection GetConnection()
        {
            if (Connection != null)
            {
                Connection.Close();
                Connection.Dispose();
                Connection = null;
            }

            Connection = new NpgsqlConnection(serverConnectionString);
            Connection.Open();

            return Connection;
        }

        protected NpgsqlCommand GetCommand(string sqlQuery, List<NpgsqlParameter> parameters = null)
        {
            NpgsqlCommand command = new NpgsqlCommand(sqlQuery);
            command.CommandType = System.Data.CommandType.Text;
            if (parameters != null)
            {
                foreach (NpgsqlParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }

        internal abstract object CreateObject(NpgsqlDataReader dr);
        internal abstract List<NpgsqlParameter> ResortObject(object instanciaClase);
        internal abstract void TestConnection();
    }
}
