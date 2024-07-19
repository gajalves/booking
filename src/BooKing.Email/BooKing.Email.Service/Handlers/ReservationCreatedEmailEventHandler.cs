using BooKing.Email.Service.Configurations;
using BooKing.Email.Service.Services;
using BooKing.Generics.Bus.Abstractions;
using BooKing.Generics.Outbox.Events;
using Microsoft.Extensions.Options;

namespace BooKing.Email.Service.Handlers;
public class ReservationCreatedEmailEventHandler : IEventHandler<ReservationCreatedEvent>
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

    public async Task<bool> Handle(ReservationCreatedEvent @event)
    {
        if (_emailServiceOptions.UseRealEmailService)
        {
            var mailTo = new List<string>
            {
                @event.UserEmail,
            };

            var subject = $"Your reservation is waiting for your confirmation!";
            var body = $@"
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
                                Reservation Confirmation Required
                            </div>
                            <div class='content'>
                                <p>Dear Customer,</p>
                                <p>Thank you for choosing our service! Your reservation details are as follows:</p>
                                <ul>
                                    <li><strong>Apartment Name:</strong> {@event.ApartmentName}</li>
                                    <li><strong>Reservation Dates:</strong> {@event.Start:MMM dd, yyyy} - {@event.End:MMM dd, yyyy}</li>
                                    <li><strong>Total Price:</strong> ${@event.TotalPrice:F2}</li>
                                </ul>
                                <p>Please confirm your reservation by clicking the link below:</p>
                                <p><a href='#' style='color: #4CAF50; text-decoration: none;'>Confirm Reservation</a></p>
                                <p>If you have any questions, feel free to contact our support team.</p>
                                <p>Best regards,</p>
                                <p>The Booking Team</p>
                            </div>
                            <div class='footer'>
                                © 2024 Booking Service. All rights reserved.
                            </div>
                        </div>
                    </body>
                    </html>";

            _sendEmailService.SendEmail(_emailServiceOptions, mailTo, subject, body, new List<string>());
        }

        await Task.Delay(3000);
        _logger.LogInformation($"Processed ReservationCreatedEvent for reservation: {@event.ReservationId}");
        return true;
    }
}
