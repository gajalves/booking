using BooKing.Identity.Application.Interfaces;
using System.Security.Cryptography;

namespace BooKing.Identity.Application.Services;
public class PasswordService : IPasswordService
{
    public string GenerateSalt(int size = 32)
    {
        var saltBytes = new byte[size];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(saltBytes);
        }
        return Convert.ToBase64String(saltBytes);
    }

    public string HashPassword(string password, string salt, int iterations = 100000)
    {
        var saltBytes = Convert.FromBase64String(salt);
        using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, iterations))
        {
            return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));
        }
    }
}
