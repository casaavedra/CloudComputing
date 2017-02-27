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
        private const string ObtenerVozPendiente = "SELECT id, urlOrigen, email FROM Pista WHERE estado = 1 LIMIT 1 FOR UPDATE SKIP LOCKED;";
        private const string ObtenerVocesPendientes = "SELECT count(id) FROM Pista WHERE estado = 1;";
        private const string ActualizarEstadoVoz = "UPDATE Pista SET estado = @estado, updatedAt = @fecha_conversion, urlFinal = @ruta_convertido WHERE id = @consecutivo";

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

        public Models.Voice GetFirstPendingConvert()
        {
            Models.Voice voice = null;

            using (NpgsqlCommand command = GetCommand(ObtenerVozPendiente))
            {
                command.Connection = Connection;
                command.Transaction = Transaction;

                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            try
                            {
                                voice = CreateObject(dr) as Models.Voice;
                            }
                            catch (Exception ex)
                            {
                                voice = new Models.Voice { State = Status.ERROR };
                                Console.WriteLine(ex.Message + " en registro " + voice.Consecutive.ToString());
                            }
                        }
                    }                    
                }
            }

            return voice;
        }

        public int Update(Models.Voice voice)
        {
            int result;                        
           
            using (NpgsqlCommand command = GetCommand(ActualizarEstadoVoz, ResortObject(voice)))
            {
                command.Connection = Connection;
                command.Transaction = Transaction;
                result = command.ExecuteNonQuery();
            }            

            return result;
        }

        public int GetCountPendingVoices()
        {
            int result;

            using (NpgsqlCommand command = GetCommand(ObtenerVocesPendientes))
            {
                command.Connection = Connection;
                command.Transaction = Transaction;
                result = int.Parse(command.ExecuteScalar().ToString());
            }

            return result;
        }

        internal override object CreateObject(NpgsqlDataReader dr)
        {
            Models.Voice voiceReg = new Models.Voice();

            voiceReg.Consecutive = dr.GetInt32(dr.GetOrdinal("id"));
            voiceReg.Origin = new FileInfo(dr.GetString(dr.GetOrdinal("urlOrigen")));
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