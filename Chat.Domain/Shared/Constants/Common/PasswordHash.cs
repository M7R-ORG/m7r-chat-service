using System.Security.Cryptography;

namespace Chat.Domain.Shared.Constants.Common;

public static class PasswordHash
{
    public const int SaltSize = 16;
    public const int Size = 32;
    public const int Iterations = 100_000;
    public static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;
}
