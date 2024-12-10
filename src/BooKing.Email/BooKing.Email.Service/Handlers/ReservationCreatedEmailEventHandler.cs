using BooKing.Email.Service.Configurations;
using BooKing.Email.Service.Services;
using BooKing.Generics.Outbox.Events;
using MassTransit;
using Microsoft.Extensions.Options;

namespace BooKing.Email.Service.Handlers;
public class ReservationCreatedEmailEventHandler : IConsumer<ReservationCreatedEvent>
{
    private readonly ILogger<Worker> _logger;
    private readonly EmailServiceOptions _emailServiceOptions;
    private readonly ISendEmailService _sendEmailService;

    public ReservationCreatedEmailEventHandler(ILogger<Worker> logger,
                                               IOptions<EmailServiceOptions> emailServiceOptions,
                                               ISendEmailService sendEmailService)
    {
        _logger = logger;
        _emailServiceOptions = emailServiceOptions.Value;
        _sendEmailService = sendEmailService;
    }

    public async Task Consume(ConsumeContext<ReservationCreatedEvent> context)
    {
        if (_emailServiceOptions.UseRealEmailService)
        {
            var mailTo = new List<string>
            {
                context.Message.UserEmail,
            };

            var subject = $"Your reservation is waiting for your confirmation!";
            var body = $@"
                        <!DOCTYPE html>
                        <html lang='en'>
                        <head>
                            <meta charset='UTF-8'>
                            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                            <title>Reservation Confirmation Required</title>
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
                                    margin: 20px 0;
                                    padding: 10px 20px;
                                    background-color: #4CAF50;
                                    color: #ffffff;
                                    text-decoration: none;
                                    border-radius: 4px;
                                    font-weight: bold;
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
                                    <h1>Reservation Confirmation Required</h1>
                                </div>
                                <div class='content'>
                                    <p>Dear Customer,</p>
                                    <p>Thank you for choosing BooKing! Your reservation details are as follows:</p>
                                    <ul>
                                        <li><strong>Apartment Name:</strong> {context.Message.ApartmentName}</li>
                                        <li><strong>Reservation Dates:</strong> {context.Message.Start:MMM dd, yyyy} - {context.Message.End:MMM dd, yyyy}</li>
                                        <li><strong>Total Price:</strong> ${context.Message.TotalPrice:F2}</li>
                                    </ul>
                                    <p>Please confirm your reservation by clicking the button below:</p>
                                    <p><a href='http://localhost:4200/profile/reservations/{context.Message.ReservationId}/details' class='button'>Confirm Reservation</a></p>
                                    <p>If you have any questions, feel free to contact our support team.</p>
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

        _logger.LogInformation($"[ReservationCreatedEmailEventHandler] Processed ReservationCreatedEvent for reservation: {context.Message.ReservationId}");
    }
}
