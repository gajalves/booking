using BooKing.Email.Service.Configurations;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;

namespace BooKing.Email.Service.Services;
public partial class SendEmailService : ISendEmailService
{
    public void SendEmail(EmailServiceOptions options, List<string> emailsTo, string subject, string body, List<string> attachments)
    {
        var setup = new Email(options.SmtpAddress, options.Email, options.Password);

        var message = PrepareteMessage(setup, emailsTo, subject, body, attachments);

        SendEmailBySmtp(setup, message);
    }

    private MailMessage PrepareteMessage(Email email, List<string> emailsTo, string subject, string body, List<string> attachments)
    {
        var mail = new MailMessage();
        mail.From = new MailAddress(email.SenderEmail);

        foreach (var mailTo in emailsTo)
        {
            if (ValidateEmail(mailTo))
            {
                mail.To.Add(mailTo);
            }
        }

        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;

        foreach (var file in attachments)
        {
            var data = new Attachment(file, MediaTypeNames.Application.Octet);
            ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(file);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(file);

            mail.Attachments.Add(data);
        }

        return mail;
    }

    private bool ValidateEmail(string email)
    {
        Regex expression = new Regex(@"\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}");
        if (expression.IsMatch(email))
            return true;

        return false;
    }

    private void SendEmailBySmtp(Email email, MailMessage message)
    {
        SmtpClient smtpClient = new SmtpClient(email.SmtpClient);
        smtpClient.Host = email.SmtpClient;
        smtpClient.Port = 587;
        smtpClient.EnableSsl = true;
        smtpClient.Timeout = 50000;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential(email.SenderEmail, email.Password);
        smtpClient.Send(message);
        smtpClient.Dispose();
    }
}
