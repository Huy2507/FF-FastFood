using System.Net;
using System.Net.Mail;
using System.Configuration;

namespace FF_Fastfood.Services
{
    public class EmailService
    {
        public void SendResetCodeEmailSMTP(string toEmail, string resetCode)
        {
            var fromAddress = new MailAddress(ConfigurationManager.AppSettings["SmtpUser"]);
            var toAddress = new MailAddress(toEmail);
            const string subject = "Reset Password";
            string body = $"Your reset code is: {resetCode}";

            var smtp = new SmtpClient
            {
                Host = ConfigurationManager.AppSettings["SmtpHost"],
                Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]),
                EnableSsl = bool.Parse(ConfigurationManager.AppSettings["SmtpEnableSsl"]),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, ConfigurationManager.AppSettings["SmtpPassword"])
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}
