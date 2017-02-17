using Isis4426.Proyecto1.ConversorBatch.Models;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Isis4426.Proyecto1.ConversorBatch.Business
{
    internal static class Configuration
    {
        private static string RutaBase = Path.GetPathRoot(Environment.SystemDirectory) + @"CloudComputing\Voces";

        internal static void Load()
        {            
            try
            {
                using (FileStream fileStream = new FileStream(@RutaBase + @"\Configuracion.xml", FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Database));
                    Database result = (Database)serializer.Deserialize(fileStream);
                    Models.Configuration.Instance.Database = result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                Models.Configuration.Instance = null;
                Save();
                Load();                
            }
        }

        internal static void Save()
        {
            try
            {
                if (!Directory.Exists(RutaBase))
                {
                    Directory.CreateDirectory(@RutaBase);
                }

                Database datos = Models.Configuration.Instance.Database;
                XmlSerializer writer = new XmlSerializer(typeof(Database));

                StreamWriter file = new StreamWriter(@RutaBase + @"\Configuracion.xml");
                writer.Serialize(file, datos);
                file.Close();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}