using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Restaurant.Backend.Common.Utils
{
    public static class PasswordUtils
    {
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return !computeHash.Where((t, i) => t != passwordHash[i]).Any();
        }

        public static string GenerateTempKey(string value)
        {
            var bytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }

            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
