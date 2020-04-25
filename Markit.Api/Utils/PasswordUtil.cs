using System;
using System.Linq;
using System.Security.Cryptography;
using Markit.Api.Interfaces.Utils;

namespace Markit.Api.Utils
{
    public class PasswordUtil : IPasswordUtil
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100;

        public string Hash(string password)
        {
            using var algorithm = new Rfc2898DeriveBytes(password, SaltSize, Iterations, HashAlgorithmName.SHA512);
            var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
            var salt = Convert.ToBase64String(algorithm.Salt);

            return $"{salt}.{key}";
        }

        public bool Verify(string hash, string password)
        {
            var parts = hash.Split('.');
            var salt = Convert.FromBase64String(parts[0]);
            var key = Convert.FromBase64String(parts[1]);
            using var algorithm = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA512);
            var keyToCheck = algorithm.GetBytes(KeySize);
            return keyToCheck.SequenceEqual(key);
        }
    }
}