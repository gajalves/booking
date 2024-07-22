using BooKing.Email.Service.Configurations;
using BooKing.Email.Service.Services;
using BooKing.Generics.Bus.Abstractions;
using BooKing.Generics.Outbox.Events;
using Microsoft.Extensions.Options;

namespace BooKing.Email.Service.Handlers;
public class NewUserEmailEventHandler : IEventHandler<NewUserEmailEvent>
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

    public async Task<bool> Handle(NewUserEmailEvent @event)
    {                
        if(_emailServiceOptions.UseRealEmailService)
        {
            var mailTo = new List<string>
            {
                @event.Email,
            };

            var subject = $"{@event.Name}, Welcome to the BooKing!";
            var body = $"Hey {@event.Name} Welcome!";

            _sendEmailService.SendEmail(_emailServiceOptions, mailTo, subject, body, new List<string>());
        }


        await Task.Delay( 3000 );
        _logger.LogInformation($"[NewUserEmailEventHandler] Processed NewUserEmailEvent for {@event.Email}");
        return true;
    }
}
