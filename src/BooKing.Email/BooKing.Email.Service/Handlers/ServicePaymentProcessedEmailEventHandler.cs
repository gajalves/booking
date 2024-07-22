using BooKing.Email.Service.Configurations;
using BooKing.Email.Service.Services;
using BooKing.Generics.Bus.Abstractions;
using BooKing.Generics.Outbox.Events;
using Microsoft.Extensions.Options;

namespace BooKing.Email.Service.Handlers;
public class ServicePaymentProcessedEmailEventHandler : IEventHandler<ReservationPaymentProcessedEvent>
{
    private readonly ILogger<Worker> _logger;
    private readonly EmailServiceOptions _emailServiceOptions;
    private readonly ISendEmailService _sendEmailService;

    public ServicePaymentProcessedEmailEventHandler(ILogger<Worker> logger,
                                                    IOptions<EmailServiceOptions> emailServiceOptions, 
                                                    ISendEmailService sendEmailService)
    {
        _logger = logger;
        _emailServiceOptions = emailServiceOptions.Value;
        _sendEmailService = sendEmailService;
    }

    public async Task<bool> Handle(ReservationPaymentProcessedEvent @event)
    {
        _logger.LogInformation($"[ServicePaymentProcessedEmailEventHandler]: Processing payment result email for ReservationId: {@event.ReservationId}");

        if (_emailServiceOptions.UseRealEmailService)
        {
            var mailTo = new List<string> { @event.UserEmail };
            string subject, body;

            if (@event.IsApproved)
            {
                subject = "Payment Confirmation - Thank you!";
                body = $@"
                    <html>
                    <head>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                color: #333;
                                line-height: 1.6;
                            }}
                            .container {{
                                width: 90%;
                                max-width: 600px;
                                margin: 0 auto;
                                border: 1px solid #ddd;
                                padding: 20px;
                                box-shadow: 2px 2px 8px rgba(0, 0, 0, 0.1);
                            }}
                            .header {{
                                background-color: #4CAF50;
                                color: white;
                                padding: 10px 0;
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
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                Payment Confirmation
                            </div>
                            <div class='content'>
                                <p>Dear Customer,</p>
                                <p>We are pleased to inform you that your payment of <strong>${@event.TotalPrice:F2}</strong> was successful.</p>
                                <p>Thank you for your reservation.</p>
                                <p>We look forward to hosting you!</p>
                                <p>Best regards,</p>
                                <p>The BooKing Team</p>
                            </div>
                            <div class='footer'>
                                © 2024 BooKing Service. All rights reserved.
                            </div>
                        </div>
                    </body>
                    </html>";
            }
            else
            {
                subject = "Payment Failed - Please Retry";
                body = $@"
                    <html>
                    <head>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                color: #333;
                                line-height: 1.6;
                            }}
                            .container {{
                                width: 90%;
                                max-width: 600px;
                                margin: 0 auto;
                                border: 1px solid #ddd;
                                padding: 20px;
                                box-shadow: 2px 2px 8px rgba(0, 0, 0, 0.1);
                            }}
                            .header {{
                                background-color: #F44336;
                                color: white;
                                padding: 10px 0;
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
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                Payment Failure
                            </div>
                            <div class='content'>
                                <p>Dear Customer,</p>
                                <p>Unfortunately, your payment of <strong>${@event.TotalPrice:F2}</strong> was not successful.</p>
                                <p>Please try again or contact our support team for assistance.</p>
                                <p>We apologize for any inconvenience.</p>
                                <p>Best regards,</p>
                                <p>The BooKing Team</p>
                            </div>
                            <div class='footer'>
                                © 2024 BooKing Service. All rights reserved.
                            </div>
                        </div>
                    </body>
                    </html>";
            }

            _sendEmailService.SendEmail(_emailServiceOptions, mailTo, subject, body, new List<string>());
        }
        
        await Task.Delay(3000);
        _logger.LogInformation($"[ServicePaymentProcessedEmailEventHandler]: Processed ReservationPaymentProcessedEvent for reservation: {@event.ReservationId}");
        return true;
    }
}
