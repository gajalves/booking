using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BooKing.Generics.Shared.CurrentUserService;
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public CurrentUser GetCurrentUser()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user.Identity.IsAuthenticated)
        {
            var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = user.FindFirst(ClaimTypes.Email)?.Value;
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            return new CurrentUser(new Guid(id), email, token);
        }

        throw new Exception("User is not authenticated");
    }    
}
