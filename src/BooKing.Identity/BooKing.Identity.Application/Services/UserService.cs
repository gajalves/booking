using BooKing.Generics.Domain;
using BooKing.Generics.Shared.CurrentUserService;
using BooKing.Identity.Application.Dtos;
using BooKing.Identity.Application.Erros;
using BooKing.Identity.Application.Interfaces;
using BooKing.Identity.Domain.Interfaces;

namespace BooKing.Identity.Application.Services;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public UserService(IUserRepository userRepository, ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<UserInformationDto>> GetUserInformation(Guid userId)
    {
        var currentUser = _currentUserService.GetCurrentUser();

        if (currentUser.Id != userId)
            return Result.Failure<UserInformationDto>(ApplicationErrors.UserError.NotAllowedToRetrieveThisInformation);

        var user = await _userRepository.GetByEmailAsync(currentUser.Email);


        var userInformation = new UserInformationDto
        {
            Email = user.Email,
            UserName = user.Name
        };

        return Result.Success(userInformation);
    }
}
