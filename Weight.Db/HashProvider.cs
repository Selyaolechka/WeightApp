using System;
using System.Security.Cryptography;
using System.Text;

namespace WeightApp.Db
{
    public interface IHashProvider : IDisposable
    {
        string GetHash(string input);
    }

    public sealed class HashProvider : IHashProvider
    {
        private readonly SHA256 _sha256Hash;

        public HashProvider()
        {
            _sha256Hash = SHA256.Create();    
        }

        public string GetHash(string input)
        {
            // Convert the input string to a byte array and compute the hash.
            var data = _sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            var sb = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            foreach (var ch in data)
            {
                sb.Append(ch.ToString("x2"));
            }

            // Return the hexadecimal string.
            return sb.ToString();
        }

        public void Dispose()
        {
            _sha256Hash?.Dispose();
        }
    }
}