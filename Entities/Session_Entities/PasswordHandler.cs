using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace QA.Entities.Session_Entities
{
    public class PasswordHandler
    {
        public static string CreatePasswordHash(string password)
        {
            return CreatePasswordHash(password, CreateSalt());
        }

        private static string CreatePasswordHash(string password, string salt)
        {
            string saltAndPwd = String.Concat(password, salt);
            string hashedPwd = GetHashString(saltAndPwd);
            var saltPosition = 5;
            hashedPwd = hashedPwd.Insert(saltPosition, salt);
            return hashedPwd;
        }

        private static bool Validate(string password, string passwordHash)
        {
            var saltPosition = 5;
            var saltSize = 10;
            var salt = passwordHash.Substring(saltPosition, saltSize);
            var hashedPassword = CreatePasswordHash(password, salt);
            return hashedPassword == passwordHash;
        }

        private static string CreateSalt()
        {
            string salt = ConfigurationManager.AppSettings["Encryption_Salt"].ToString();
            return salt.ToUpper();

            //RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            //byte[] buff = new byte[20];
            //rng.GetBytes(buff);
            //var saltSize = 10;
            //string salt = Convert.ToBase64String(buff);
            //if (salt.Length > saltSize)
            //{
            //    salt = salt.Substring(0, saltSize);
            //    return salt.ToUpper();
            //}

            //var saltChar = '^';
            //salt = salt.PadRight(saltSize, saltChar);
            //return salt.ToUpper();
        }

        private static string GetHashString(string password)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(password))
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }

        private static byte[] GetHash(string password)
        {
            SHA384 sha = new SHA384CryptoServiceProvider();
            return sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}