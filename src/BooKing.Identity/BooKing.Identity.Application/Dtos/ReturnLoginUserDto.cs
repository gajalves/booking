namespace BooKing.Identity.Application.Dtos;
public class ReturnLoginUserDto
{
    public string AccessToken { get; set; }
    public double ExpiresIn { get; set; }    

    public Guid UserId { get; set; }
    public string UserEmail { get; set; }
    public string UserName { get; set; }
}
