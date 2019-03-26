using System;
using System.Net;
using System.Net.Mail;

namespace WebApp.Web.Infrastructure.Functions
{
    public class SettingFunction
    {
        // Gửi email
        public static bool SendMail(string name, string subject, string content, string toMail)
        {
            bool rs = false;
            try
            {
                var message = new MailMessage();
                var smtp = new System.Net.Mail.SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com"; //host name
                    smtp.Port = 587; //port number
                    smtp.EnableSsl = true; //whether your smtp server requires SSL
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential("shopinoxthaian@gmail.com", "@123456ok");
                    smtp.Timeout = 3600;
                }

                MailAddress fromAddress = new MailAddress("shopinoxthaian@gmail.com", name);
                message.From = fromAddress;
                message.To.Add(toMail);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = content;
                smtp.Send(message);
                rs = true;
            }
            catch (Exception)
            {
                rs = false;
            }

            return rs;
        }
    }
}