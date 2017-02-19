using System;
using System.Collections.Generic;
using Npgsql;
using Isis4426.Proyecto1.ConversorBatch.Interfaces;
using Isis4426.Proyecto1.ConversorBatch.Models;
using System.IO;

namespace Isis4426.Proyecto1.ConversorBatch.Data_Access
{
    class Voice : PostgreSqlConnector, IVoiceCrud
    {
        public List<Models.Voice> PendingConvert()
        {
            string path = Path.Combine(Configuration.Instance.BasePath, "Sentencias", "ObtenerVocesPendientes.txt");
            string query = File.ReadAllText(path);
            List<Models.Voice> pendingVoices = new List<Models.Voice>();

            using (GetConnection())
            {
                using (NpgsqlCommand comando = GetCommand(query))
                {
                    comando.Connection = Connection;
                    using (NpgsqlDataReader dr = comando.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Models.Voice voice = null;

                                try
                                {
                                    voice = CreateObject(dr) as Models.Voice;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message + " en registro " + voice.Consecutive.ToString());
                                    voice.State = Status.ERROR;
                                }

                                pendingVoices.Add(voice);
                            }
                        }
                    }
                }
            }

            return pendingVoices;
        }

        public int Update(Models.Voice voice)
        {
            int result;
            string path = Path.Combine(Configuration.Instance.BasePath, "Sentencias", "ActualizarEstadoVoz.txt");
            string query = File.ReadAllText(path);
                        
            using (GetConnection())
            {
                using (NpgsqlCommand comando = GetCommand(query, ResortObject(voice)))
                {
                    comando.Connection = Connection;
                    result = comando.ExecuteNonQuery();
                }
            }

            return result;
        }

        internal override object CreateObject(NpgsqlDataReader dr)
        {
            Models.Voice voiceReg = new Models.Voice();

            voiceReg.Consecutive = dr.GetInt32(dr.GetOrdinal("consecutivo"));
            voiceReg.Origin = new FileInfo(dr.GetString(dr.GetOrdinal("ruta_original")));

            return voiceReg;
        }

        internal override List<NpgsqlParameter> ResortObject(object instanciaClase)
        {
            Models.Voice voice = instanciaClase as Models.Voice;

            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Add(new NpgsqlParameter("consecutivo", voice.Consecutive));
            parameters.Add(new NpgsqlParameter("estado", (int)Status.GENERATED));
            parameters.Add(new NpgsqlParameter("fecha_convesion", DateTime.Now));
            parameters.Add(new NpgsqlParameter("ruta_convertido", voice.Destiny.FullName));

            return parameters;
        }

        internal override void TestConnection()
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }
            }
        }
    }
}