using BooKing.Email.Service.Configurations;
using BooKing.Email.Service.Services;
using BooKing.Generics.Outbox.Events;
using MassTransit;
using Microsoft.Extensions.Options;

namespace BooKing.Email.Service.Handlers;
public class NewUserEmailEventHandler : IConsumer<NewUserEmailEvent>
{
    private readonly ILogger<Worker> _logger;
    private readonly EmailServiceOptions _emailServiceOptions;
    private readonly ISendEmailService _sendEmailService;

    public NewUserEmailEventHandler(ILogger<Worker> logger, 
                                    IOptions<EmailServiceOptions> emailServiceOptions, 
                                    ISendEmailService sendEmailService)
    {
        _logger = logger;
        _emailServiceOptions = emailServiceOptions.Value;
        _sendEmailService = sendEmailService;
    }

    public async Task Consume(ConsumeContext<NewUserEmailEvent> context)
    {                
        if(_emailServiceOptions.UseRealEmailService)
        {
            var mailTo = new List<string>
            {
                context.Message.Email
            };

            var subject = $"{context.Message.Name}, Welcome to the BooKing!";
            var body = $@"
                        <!DOCTYPE html>
                        <html lang='en'>
                        <head>
                            <meta charset='UTF-8'>
                            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                            <title>Welcome to BooKing!</title>
                            <style>
                                body {{
                                    font-family: Arial, sans-serif;
                                    background-color: #f9f9f9;
                                    margin: 0;
                                    padding: 0;
                                }}
                                .email-container {{
                                    max-width: 600px;
                                    margin: 20px auto;
                                    background-color: #ffffff;
                                    border: 1px solid #ddd;
                                    border-radius: 8px;
                                    overflow: hidden;
                                }}
                                .header {{
                                    background-color: #007bff;
                                    color: #ffffff;
                                    text-align: center;
                                    padding: 20px;
                                }}
                                .header h1 {{
                                    margin: 0;
                                    font-size: 24px;
                                }}
                                .content {{
                                    padding: 20px;
                                    color: #333333;
                                }}
                                .content h2 {{
                                    font-size: 20px;
                                    margin-top: 0;
                                }}
                                .content p {{
                                    line-height: 1.6;
                                }}
                                .button {{
                                    display: inline-block;
                                    margin-top: 20px;
                                    padding: 10px 20px;
                                    background-color: #007bff;
                                    color: #ffffff;
                                    text-decoration: none;
                                    border-radius: 4px;
                                }}
                                .footer {{
                                    background-color: #f1f1f1;
                                    text-align: center;
                                    padding: 10px;
                                    font-size: 12px;
                                    color: #777777;
                                }}
                            </style>
                        </head>
                        <body>
                            <div class='email-container'>
                                <div class='header'>
                                    <h1>Welcome to BooKing!</h1>
                                </div>
                                <div class='content'>
                                    <h2>Hello, {context.Message.Name}!</h2>
                                    <p>
                                        Thank you for joining <strong>BooKing</strong>. We're excited to have you on board! 
                                        BooKing makes finding and reserving your perfect apartment easy and hassle-free.
                                    </p>
                                    <p>
                                        Explore our platform and find your next stay with us. If you have any questions, 
                                        feel free to reach out—we're here to help!
                                    </p>
                                    <a href='https://yourwebsite.com' class='button'>Get Started</a>
                                </div>
                                <div class='footer'>
                                    <p>
                                        © 2024 BooKing. All rights reserved.
                                        <br>
                                        If you did not create this account, please disregard this email.
                                    </p>
                                </div>
                            </div>
                        </body>
                        </html>";

            _sendEmailService.SendEmail(_emailServiceOptions, mailTo, subject, body, new List<string>());
        }

        _logger.LogInformation($"[NewUserEmailEventHandler] Processed NewUserEmailEvent for {context.Message.Email}");        
    }   
}
