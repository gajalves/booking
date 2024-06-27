using BooKing.Email.Service.Configurations;

namespace BooKing.Email.Service.Services;
public interface ISendEmailService
{
    void SendEmail(EmailServiceOptions options, List<string> emailsTo, string subject, string body, List<string> attachments);    
}
