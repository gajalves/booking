using AutoMapper;
using BooKing.Generics.Domain;
using BooKing.Identity.Application.Dtos;
using BooKing.Identity.Application.Erros;
using BooKing.Identity.Application.Interfaces;
using BooKing.Identity.Domain.Entities;
using BooKing.Identity.Domain.Interfaces;

namespace BooKing.Identity.Application.Services;
public class UserRegisterService : IUserRegisterService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IMapper _mapper;

    public UserRegisterService(IUserRepository userRepository, IPasswordService passwordService, IMapper mapper)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _mapper = mapper;
    }

    public async Task<Result<ReturnCreatedUserDto>> Register(UserRegisterDto dto)
    {
        var emailAlreadyInUse = await _userRepository.EmailAlreadyInUse(dto.Email);
        
        if (emailAlreadyInUse)
            return Result.Failure<ReturnCreatedUserDto>(ApplicationErrors.UserError.EmailAlreadyInUse);

        var salt = _passwordService.GenerateSalt();
        var passwordHash = _passwordService.HashPassword(dto.Password, salt);

        var newUser = new User(dto.Email, dto.Name, passwordHash, salt);
        
        var createdUser = await _userRepository.CreateAsync(newUser);

        return _mapper.Map<ReturnCreatedUserDto>(createdUser);
    }   
}
