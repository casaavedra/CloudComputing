using System;
using System.Net;
using System.Net.Mail;

namespace Isis4426.Proyecto1.ConversorBatch.Business
{
    internal static class Email
    {
        internal static void Send(Models.Voice voice)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(Models.Configuration.Instance.EmailServer.SmtpServer);

                mail.From = new MailAddress(Models.Configuration.Instance.EmailServer.From);
                mail.To.Add(voice.Email);
                mail.Subject = Models.Configuration.Instance.EmailServer.Subject;
                mail.Body = Models.Configuration.Instance.EmailServer.Body;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new NetworkCredential("username", "password");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}