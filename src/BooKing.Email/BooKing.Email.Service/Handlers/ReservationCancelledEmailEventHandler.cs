using BooKing.Email.Service.Configurations;
using BooKing.Email.Service.Services;
using BooKing.Generics.Outbox.Events;
using MassTransit;
using Microsoft.Extensions.Options;

namespace BooKing.Email.Service.Handlers;
public class ReservationCancelledEmailEventHandler : IConsumer<ReservationCancelledByUserEvent>
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

    public async Task Consume(ConsumeContext<ReservationCancelledByUserEvent> context)
    {
        if (_emailServiceOptions.UseRealEmailService)
        {
            var mailTo = new List<string> { context.Message.UserEmail };
            var subject = $"Your Reservation Has Been Cancelled";
            var body = $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Reservation Cancellation Notice</title>
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
                            background-color: #E53935;
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
                        .content p {{
                            line-height: 1.6;
                        }}
                        .content ul {{
                            margin: 10px 0;
                            padding-left: 20px;
                        }}
                        .content li {{
                            margin-bottom: 10px;
                        }}
                        .button {{
                            display: inline-block;
                            margin-top: 20px;
                            padding: 10px 20px;
                            background-color: #E53935;
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
                            <h1>Reservation Cancellation Notice</h1>
                        </div>
                        <div class='content'>
                            <p>Dear Customer,</p>
                            <p>We regret to inform you that your reservation has been cancelled. Below are the details of your cancelled reservation:</p>
                            <ul>
                                <li><strong>Original Reservation Dates:</strong> {context.Message.Start:MMM dd, yyyy} - {context.Message.End:MMM dd, yyyy}</li>
                                <li><strong>Total Price:</strong> ${context.Message.TotalPrice:F2}</li>
                            </ul>
                            <p>If you have any questions or need further assistance, please do not hesitate to contact our support team.</p>
                            <p>We hope to serve you again in the future.</p>
                            <p>Best regards,</p>
                            <p>The BooKing Team</p>
                        </div>
                        <div class='footer'>
                            <p>© 2024 BooKing Service. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>";

            _sendEmailService.SendEmail(_emailServiceOptions, mailTo, subject, body, new List<string>());
        }

        _logger.LogInformation($"[ReservationCancelledEmailEventHandler] Processed ReservationCreatedEvent for reservation: {context.Message.ReservationId}");        
    }
}
