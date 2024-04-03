using System.Collections.Generic;
using System.Net.Mail;
using System.Net;

namespace WebBanNoiThat
{
    public class MailService
    {
   
        private const string MAIL = "aitrinh0603@gmail.com";
        private const string PASSWORD = "muqdosqjzuivfwtu";
        private SmtpClient smtpClient;

        public MailService()
        {
            smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587; // Port của SMTP server
            smtpClient.Credentials = new NetworkCredential(MAIL, PASSWORD);
            smtpClient.EnableSsl = true;
        }

        public void SendMail(
            IEnumerable<string> to,
            string subject,
            string content,
            bool isBodyHtml = false)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(MAIL);

            foreach (var reciever in to)
            {
                message.To.Add(reciever);
            }

            message.Subject = subject;
            message.Body = content;
            message.IsBodyHtml = true;

            smtpClient.Send(message);
        }
    }
}
