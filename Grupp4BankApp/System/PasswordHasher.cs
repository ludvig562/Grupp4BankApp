using Grupp4BankApp.Users;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Grupp4BankApp.System
{
    internal class PasswordHasher
    {
        // Method used for hashing passwords before saving them
        public static string HashPassword(string password)
        {
            // Generates a random 16-byte salt
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            // Creates a hash from the password and salt using PBKDF2 with SHA256
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                600000,
                HashAlgorithmName.SHA256,
                32
            );

            // Converts the salt and hash to Base64 strings and separates them with :
            return Convert.ToBase64String(salt) + ":" +
                   Convert.ToBase64String(hash);
        }

        // Method used to verify an entered password against a saved password hash
        public static bool VerifyPassword(string password, string storedPassword) 
        {
            // Splits the stored string at : into two array elements
            string[] parts = storedPassword.Split(':');

            // Converts the first element (salt) from Base64 to a byte array
            byte[] salt = Convert.FromBase64String(parts[0]);

            // Converts the second element (stored hash) from Base64 to a byte array
            byte[] storedHash = Convert.FromBase64String(parts[1]);

            // Creates a new hash using the entered password and the saved salt
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                600000,
                HashAlgorithmName.SHA256,
                32
            );

            // Compares both hashes and returns true if they match
            return CryptographicOperations.FixedTimeEquals(hash, storedHash);
        }
    }
}
