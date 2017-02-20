using System;
using System.Net;
using System.Net.Mail;

namespace Isis4426.Proyecto1.ConversorBatch.Business
{
    internal static class Email
    {
        internal static bool Send(Models.Voice voice)
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
                SmtpServer.Credentials = new NetworkCredential("ksaavedra3", "beto4427247");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                return true;             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
    }
}