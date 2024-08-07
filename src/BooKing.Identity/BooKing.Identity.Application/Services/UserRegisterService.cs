using AutoMapper;
using BooKing.Generics.Bus.Queues;
using BooKing.Generics.Domain;
using BooKing.Generics.Outbox.Events;
using BooKing.Generics.Outbox.Service;
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
    private readonly IOutboxEventService _outboxEventService;
    private readonly IMapper _mapper;

    public UserRegisterService(IUserRepository userRepository, 
                               IPasswordService passwordService, 
                               IMapper mapper, 
                               IOutboxEventService outboxEventService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _mapper = mapper;
        _outboxEventService = outboxEventService;
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

        if(createdUser == null)
            return Result.Failure<ReturnCreatedUserDto>(ApplicationErrors.UserError.UnexpectedErrorCreatingUser);

        await _outboxEventService.AddEvent(QueueMapping.BooKingEmailServiceNewUser, new NewUserEmailEvent(newUser.Id, dto.Name, dto.Email));

        var newUserDto = _mapper.Map<ReturnCreatedUserDto>(createdUser);

        return Result.Success(newUserDto);
    }   
}
