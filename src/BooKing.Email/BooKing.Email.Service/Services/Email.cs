namespace BooKing.Email.Service.Services;
public class Email
{
    public Email(string smtpClient, string senderEmail, string password, string from, string to)
    {
        SmtpClient = smtpClient;
        SenderEmail = senderEmail;
        Password = password;
        From = from;
        To = to;
    }

    public string SmtpClient { get; private set; }
    public string SenderEmail { get; private set; }
    public string Password { get; private set; }
    public string From { get; private set; }
    public string To { get; private set; }

}
