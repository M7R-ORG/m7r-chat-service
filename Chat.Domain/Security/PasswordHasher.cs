using System.Security.Cryptography;
using System.Text;
using Chat.Domain.Exceptions;
using Chat.Domain.Shared.Constants.Common;
using Chat.Domain.Shared.Models;

namespace Chat.Domain.Security;

public static class PasswordHasher
{
    public static Password Create(string password)
    {
        try
        {
            byte[] salt = RandomNumberGenerator.GetBytes(PasswordHash.SaltSize);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                PasswordHash.Iterations,
                PasswordHash.Algorithm,
                PasswordHash.Size
            );

            return new Password() { Salt = salt, Hash = hash };
        }
        catch (Exception)
        {
            throw new FailedToCreatePasswordException();
        }
    }
}
