using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Grupp4BankApp.System
{
    internal class AuthenticationService
    {

    public int FailedLogin { get; set; }
        public List<BankAccount> Accounts { get; set; } = new();
        public const string masterkey = "SYML";

        public bool IsLocked => FailedLogin >= 3;


        public static string Hash(string password) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(masterkey + password)));

        public bool CheckPassword(string password)
        {
            if (IsLocked) return false;

            if (Hash(password) == Password)
            {
                FailedLogin = 0;
                return true;
            }

            FailedLogin++;
            return false;
        }
    }
