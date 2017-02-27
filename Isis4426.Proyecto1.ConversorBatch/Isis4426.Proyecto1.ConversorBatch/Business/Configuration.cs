using System;
using System.IO;
using System.Xml.Serialization;

namespace Isis4426.Proyecto1.ConversorBatch.Business
{
    internal static class Configuration
    {
        private static string basePath = Path.GetPathRoot(Environment.SystemDirectory) + @"CloudComputing\Voces";

        internal static void Load()
        {            
            try
            {
                Models.Configuration.Instance = null;
                Models.Configuration.Instance.BasePath = basePath;

                /*using (FileStream fileStream = new FileStream(basePath + @"\Configuracion.xml", FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Models.Configuration));
                    Models.Configuration.Instance = (Models.Configuration)serializer.Deserialize(fileStream);
                    Models.Configuration.Instance.BasePath = basePath;
                }*/
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                /*Models.Configuration.Instance = null;
                Save();*/
            }
        }

        internal static void Save()
        {
            try
            {
                if (!Directory.Exists(Models.Configuration.Instance.BasePath))
                {
                    Directory.CreateDirectory(Models.Configuration.Instance.BasePath);
                }
                                
                XmlSerializer writer = new XmlSerializer(typeof(Models.Configuration));

                StreamWriter file = new StreamWriter(Models.Configuration.Instance.BasePath + @"\Configuracion.xml");
                writer.Serialize(file, Models.Configuration.Instance);
                file.Close();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}