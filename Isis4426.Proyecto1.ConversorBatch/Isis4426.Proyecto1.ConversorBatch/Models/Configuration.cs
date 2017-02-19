using System;
using System.IO;
using System.Xml.Serialization;

namespace Isis4426.Proyecto1.ConversorBatch.Models
{
    public class Configuration
    {
        private static Configuration instance;

        public Database Database { get; set; }
        public EmailServer EmailServer { get; set; }
        public int ConvertTimer { get; set; }
        [XmlIgnore]
        public string BasePath { get; set; }

        private Configuration()
        {
            Database = new Database
            {
                Server = "localhost",
                Port = 5432,
                Username = "postgres",
                Password = "agiles",
                Schema = "cloud"
            };

            EmailServer = new EmailServer
            {
                SmtpServer = "smtp.gmail.com",
                From = "ksaavedra3@gmail.com",
                Subject = "Voz publicada",
                Body = "Su voz ha sido Publicada en la página del concurso.",
                Username = "ksaavedra3@gmail.com",
                Password = "beto4427247"
            };

            BasePath = Path.GetPathRoot(Environment.SystemDirectory) + @"CloudComputing\Voces";
            ConvertTimer = 2000;
        }

        public static Configuration Instance
        {
            get
            {
                return instance;
            }
            set
            {
                if (value != null)
                    instance = value;
                else
                    instance = new Configuration();
            }
        }
    }
}