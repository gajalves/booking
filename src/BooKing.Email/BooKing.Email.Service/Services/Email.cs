namespace BooKing.Email.Service.Services;
public class Email
{
    public Email(string smtpClient, string senderEmail, string password)
    {
        SmtpClient = smtpClient;
        SenderEmail = senderEmail;
        Password = password;
    }

    public string SmtpClient { get; private set; }
    public string SenderEmail { get; private set; }
    public string Password { get; private set; }

}
