namespace BooKing.Identity.Application.Dtos;
public class ResponseGenerateTokenDto
{
    public string AccessToken { get; set; }
    public double ExpiresIn { get; set; }
}
