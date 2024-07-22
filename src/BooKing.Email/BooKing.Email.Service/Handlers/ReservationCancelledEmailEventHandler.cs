using BooKing.Email.Service.Configurations;
using BooKing.Email.Service.Services;
using BooKing.Generics.Bus.Abstractions;
using BooKing.Generics.Outbox.Events;
using Microsoft.Extensions.Options;

namespace BooKing.Email.Service.Handlers;
public class ReservationCancelledEmailEventHandler : IEventHandler<ReservationCancelledByUserEvent>
{
    private readonly ILogger<Worker> _logger;
    private readonly EmailServiceOptions _emailServiceOptions;
    private readonly ISendEmailService _sendEmailService;

    public ReservationCancelledEmailEventHandler(ILogger<Worker> logger,
                                               IOptions<EmailServiceOptions> emailServiceOptions,
                                               ISendEmailService sendEmailService)
    {
        _logger = logger;
        _emailServiceOptions = emailServiceOptions.Value;
        _sendEmailService = sendEmailService;
    }

    public async Task<bool> Handle(ReservationCancelledByUserEvent @event)
    {
        if (_emailServiceOptions.UseRealEmailService)
        {
            var mailTo = new List<string> { @event.UserEmail };
            var subject = $"Your Reservation Has Been Cancelled";
            var body = $@"
                    <html>
                    <head>
                        <style>
                            body {{
                                font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                                color: #444;
                                background-color: #f9f9f9;
                                margin: 0;
                                padding: 0;
                            }}
                            .container {{
                                width: 90%;
                                max-width: 600px;
                                margin: 20px auto;
                                background-color: #fff;
                                border-radius: 8px;
                                box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                                overflow: hidden;
                            }}
                            .header {{
                                background-color: #E53935;
                                color: white;
                                padding: 15px 0;
                                text-align: center;
                                font-size: 24px;
                            }}
                            .content {{
                                padding: 20px;
                            }}
                            .footer {{
                                text-align: center;
                                margin-top: 20px;
                                font-size: 12px;
                                color: #777;
                                padding-bottom: 20px;
                            }}
                            .button {{
                                display: inline-block;
                                margin-top: 20px;
                                padding: 10px 20px;
                                color: white;
                                background-color: #E53935;
                                text-decoration: none;
                                border-radius: 4px;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                Reservation Cancellation Notice
                            </div>
                            <div class='content'>
                                <p>Dear Customer,</p>
                                <p>We regret to inform you that your reservation has been cancelled. Below are the details of your cancelled reservation:</p>
                                <ul>                                    
                                    <li><strong>Original Reservation Dates:</strong> {@event.Start:MMM dd, yyyy} - {@event.End:MMM dd, yyyy}</li>
                                    <li><strong>Total Price:</strong> ${@event.TotalPrice:F2}</li>
                                </ul>
                                <p>If you have any questions or need further assistance, please do not hesitate to contact our support team.</p>
                                <p>We hope to serve you again in the future.</p>
                                <p>Best regards,</p>
                                <p>The BooKing Team</p>                                
                            </div>
                            <div class='footer'>
                                © 2024 BooKing Service. All rights reserved.
                            </div>
                        </div>
                    </body>
                    </html>";

            _sendEmailService.SendEmail(_emailServiceOptions, mailTo, subject, body, new List<string>());
        }

        await Task.Delay(3000);
        _logger.LogInformation($"[ReservationCancelledEmailEventHandler] Processed ReservationCreatedEvent for reservation: {@event.ReservationId}");
        return true;
    }
}
