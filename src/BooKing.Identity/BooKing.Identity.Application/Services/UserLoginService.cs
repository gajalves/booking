using AutoMapper;
using BooKing.Generics.Domain;
using BooKing.Identity.Application.Dtos;
using BooKing.Identity.Application.Erros;
using BooKing.Identity.Application.Interfaces;
using BooKing.Identity.Domain.Interfaces;

namespace BooKing.Identity.Application.Services;
public class UserLoginService : IUserLoginService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;

    public UserLoginService(IUserRepository userRepository,
                            IPasswordService passwordService,
                            ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _tokenService = tokenService;
    }

    public async Task<Result<ReturnLoginUserDto>> Login(UserLoginDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user == null)
            return Result.Failure<ReturnLoginUserDto>(ApplicationErrors.UserError.ProvidedEmailAccountNotFound);

        var hash = GenerateHash(dto.Password, user.UserSalt.Salt);

        if (user.Password != hash)
            return Result.Failure<ReturnLoginUserDto>(ApplicationErrors.UserError.PasswordIncorrect);

        return CreateLoginReturn(user.Id, user.Email, user.Name);
    }

    private Result<ReturnLoginUserDto> CreateLoginReturn(Guid id, string email, string name)
    {
        var token =  _tokenService.GenerateToken(id, email);

        return new ReturnLoginUserDto
        {
            AccessToken = token.AccessToken,
            ExpiresIn = token.ExpiresIn,
            UserEmail = email,
            UserId = id,
            UserName = name
        };
        
    }

    private string GenerateHash(string dtoPassword, string salt)
    {
        return _passwordService.HashPassword(dtoPassword, salt);
    }
}
