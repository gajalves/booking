namespace BooKing.Email.Service.Configurations;
public class EmailServiceOptions
{
    public bool UseRealEmailService { get; set; }
    
    public string SmtpAddress { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
