namespace BooKing.Identity.Application.Interfaces;
public interface IPasswordService
{
    string GenerateSalt(int size = 32);
    string HashPassword(string password, string salt, int iterations = 100000);
}
