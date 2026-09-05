using Microsoft.AspNetCore.Connections;
using System.Net;
using System.Net.Mail;
namespace IdentityCoreDemo.Serviecs
{
    public class EmailService:IEmailServices
    {
        public bool SendMail(string email, string subject, string message)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient= new SmtpClient();
                mailMessage.From = new MailAddress("vikashsinghtetulmari@gmail.com");
                mailMessage.Subject = subject;
                mailMessage.To.Add(email);
               
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = true;

                smtpClient.Port = 587;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("vikashsinghtetulmari@gmail.com", "fpsn wukq qnyt rozd");
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
               
            }
            return false;
        }
    }
}
