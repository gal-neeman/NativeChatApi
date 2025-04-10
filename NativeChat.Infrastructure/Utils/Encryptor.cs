using System.Security.Cryptography;
using System.Text;

namespace NativeChat;

public static class Encryptor
{
    public static string GetHashed(string originalText)
    {
        const string salt = "Why would you even want to learn japanese";
        var saltBytes = Encoding.UTF8.GetBytes(salt);
        var rfc = new Rfc2898DeriveBytes(originalText, saltBytes, 14, HashAlgorithmName.SHA512);
        var hashByte = rfc.GetBytes(64);
        var hashPassword = Convert.ToBase64String(hashByte);

        return hashPassword;
    }
}
