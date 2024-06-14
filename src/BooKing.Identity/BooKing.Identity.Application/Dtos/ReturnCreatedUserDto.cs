namespace BooKing.Identity.Application.Dtos;
public class ReturnCreatedUserDto
{
    public Guid Id { get; set; }
    public string Email { get; private set; }
    public string Name { get; private set; }
}
