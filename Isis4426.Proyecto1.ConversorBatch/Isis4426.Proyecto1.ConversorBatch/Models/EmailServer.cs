﻿namespace Isis4426.Proyecto1.ConversorBatch.Models
{
    public class EmailServer
    {
        public string SmtpServer { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
