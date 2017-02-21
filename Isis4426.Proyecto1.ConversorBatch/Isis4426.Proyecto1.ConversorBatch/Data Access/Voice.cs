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
        private const string ObtenerVocesPendientes = "SELECT consecutivo, ruta_original, email FROM voces inner join usuarios on documento = usuario WHERE estado = 1;";
        private const string ActualizarEstadoVoz = "UPDATE voces SET estado = @estado, fecha_conversion = @fecha_conversion, ruta_convertido = @ruta_convertido WHERE consecutivo = @consecutivo";

        internal static Voice Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        instance = new Voice();
                    }
                }

                return (Voice)instance;
            }
        }
        
        public List<Models.Voice> PendingConvert()
        {
            List<Models.Voice> pendingVoices = new List<Models.Voice>();
            
            using (NpgsqlCommand comando = GetCommand(ObtenerVocesPendientes))
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
            
            return pendingVoices;
        }

        public int Update(Models.Voice voice)
        {
            int result;                        
           
            using (NpgsqlCommand comando = GetCommand(ActualizarEstadoVoz, ResortObject(voice)))
            {
                comando.Connection = Connection;
                result = comando.ExecuteNonQuery();
            }            

            return result;
        }

        internal override object CreateObject(NpgsqlDataReader dr)
        {
            Models.Voice voiceReg = new Models.Voice();

            voiceReg.Consecutive = dr.GetInt32(dr.GetOrdinal("consecutivo"));
            voiceReg.Origin = new FileInfo(dr.GetString(dr.GetOrdinal("ruta_original")));
            voiceReg.Email = dr.GetString(dr.GetOrdinal("email"));

            return voiceReg;
        }

        internal override List<NpgsqlParameter> ResortObject(object instanciaClase)
        {
            Models.Voice voice = instanciaClase as Models.Voice;

            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Add(new NpgsqlParameter("consecutivo", voice.Consecutive));
            parameters.Add(new NpgsqlParameter("estado", (int)Status.GENERATED));
            parameters.Add(new NpgsqlParameter("fecha_conversion", DateTime.Now));
            parameters.Add(new NpgsqlParameter("ruta_convertido", voice.Destiny.FullName));

            return parameters;
        }

        internal override void TestConnection()
        {
            using (NpgsqlConnection conn = Connection)
            {
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }
            }
        }
    }
}