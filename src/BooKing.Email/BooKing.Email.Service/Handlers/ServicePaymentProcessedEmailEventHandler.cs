using BooKing.Email.Service.Configurations;
using BooKing.Email.Service.Services;
using BooKing.Generics.Outbox.Events;
using MassTransit;
using Microsoft.Extensions.Options;

namespace BooKing.Email.Service.Handlers;
public class ServicePaymentProcessedEmailEventHandler : IConsumer<ReservationPaymentProcessedEvent>
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

    public async Task Consume(ConsumeContext<ReservationPaymentProcessedEvent> context)
    {
        _logger.LogInformation($"[ServicePaymentProcessedEmailEventHandler]: Processing payment result email for ReservationId: {context.Message.ReservationId}");

        if (_emailServiceOptions.UseRealEmailService)
        {
            var mailTo = new List<string> { context.Message.UserEmail };
            string subject, body;

            if (context.Message.IsApproved)
            {
                subject = "Payment Confirmation - Thank you!";
                body = $@"
                    <!DOCTYPE html>
                    <html lang='en'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>Payment Confirmation</title>
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
                                background-color: #4CAF50;
                                color: white;
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
                                <h1>Payment Confirmation</h1>
                            </div>
                            <div class='content'>
                                <p>Dear Customer,</p>
                                <p>We are pleased to inform you that your payment of <strong>${context.Message.TotalPrice:F2}</strong> was successful.</p>
                                <p>Thank you for your reservation.</p>
                                <p>We look forward to hosting you!</p>
                                <p>Best regards,</p>
                                <p>The BooKing Team</p>
                            </div>
                            <div class='footer'>
                                <p>© 2024 BooKing Service. All rights reserved.</p>
                            </div>
                        </div>
                    </body>
                    </html>";
            }
            else
            {
                subject = "Payment Failed - Please Retry";
                body = $@"
                    <!DOCTYPE html>
                    <html lang='en'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>Payment Failure</title>
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
                                background-color: #F44336;
                                color: white;
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
                                <h1>Payment Failure</h1>
                            </div>
                            <div class='content'>
                                <p>Dear Customer,</p>
                                <p>Unfortunately, your payment of <strong>${context.Message.TotalPrice:F2}</strong> was not successful.</p>
                                <p>Please try again or contact our support team for assistance.</p>
                                <p>We apologize for any inconvenience.</p>
                                <p>Best regards,</p>
                                <p>The BooKing Team</p>
                            </div>
                            <div class='footer'>
                                <p>© 2024 BooKing Service. All rights reserved.</p>
                            </div>
                        </div>
                    </body>
                    </html>";
            }

            _sendEmailService.SendEmail(_emailServiceOptions, mailTo, subject, body, new List<string>());
        }

        _logger.LogInformation($"[ServicePaymentProcessedEmailEventHandler]: Processed ReservationPaymentProcessedEvent for reservation: {context.Message.ReservationId}");
    }
}
