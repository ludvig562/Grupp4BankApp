using Grupp4BankApp.Users;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Grupp4BankApp.System
{
    internal class PasswordHasher
    {
        public static string HashPassword(string password)// Method used for hashing saved paswords
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            // method that dose a mathematical formula based on the algorith salt and the pasword and prints out a hash
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                600000,
                HashAlgorithmName.SHA256,
                32
            );

            //creates a string with the salt and hash and puts a : in betwen
            return Convert.ToBase64String(salt) + ":" +
                   Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(string password, string storedPassword) // method to verify that entered pasword has the same hash as the saved pasword
        {
            string[] parts = storedPassword.Split(':');// splits the string where the : starts and creates 2 array elements

            byte[] salt = Convert.FromBase64String(parts[0]);// takes the first element and ads the content to a byte array
            byte[] storedHash = Convert.FromBase64String(parts[1]);// takes the second element in the array and creates a byte array

            // method that dose a mathematical formula based on the algorith salt and the pasword and returns true if the saved hash is the same as the entered pasword hash
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                600000,
                HashAlgorithmName.SHA256,
                32
            );

            return CryptographicOperations.FixedTimeEquals(hash, storedHash);
        }
    }
}
