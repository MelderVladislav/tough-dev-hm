using System.Net;
using System.Net.Mail;

namespace UberPopug.Domains.Auth.Services.Identity;

internal class EmailService
{
    public void SendEmail(string from,
        string to,
        string fromPassword,
        string subject,
        string host,
        int port,
        string htmlContent,
        bool enableSsl)
    {
        var message = new MailMessage();
        var smtp = new SmtpClient(host, port);
        message.From = new MailAddress(from);
        message.To.Add(new MailAddress(to));
        message.Subject = subject;
        message.IsBodyHtml = true;
        message.Body = htmlContent;
        smtp.EnableSsl = enableSsl;
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new NetworkCredential(from, fromPassword);
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

        smtp.Send(message);
    }
}