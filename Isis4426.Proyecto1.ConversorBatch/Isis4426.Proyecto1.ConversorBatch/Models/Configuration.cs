namespace Isis4426.Proyecto1.ConversorBatch.Models
{
    internal class Configuration
    {
        private static Configuration instance;

        public Database Database { get; set; }

        public int ConvertTimer { get; set; }

        private Configuration()
        {
            Database = new Database
            {
                Server = "localhost",
                Port = 5432,
                Username = "postgress",
                Password = "identica",
                Schema = "proyecto1"
            };

            ConvertTimer = 1000;
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