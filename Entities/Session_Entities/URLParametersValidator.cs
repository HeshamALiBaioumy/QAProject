using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace QA.Entities.Session_Entities
{
    public static class URLParametersValidator
    {
        private static byte[] _optionalEntropy = { 9, 8, 7, 6, 5 };

        public static string Encrypt(string plainText)
        {
            return HttpServerUtility.UrlTokenEncode(ProtectedData.Protect(Encoding.UTF8.GetBytes(plainText)
                , _optionalEntropy, DataProtectionScope.LocalMachine));
        }

        public static string Decrypt(string text)
        {
            return Encoding.UTF8.GetString(ProtectedData.Unprotect(HttpServerUtility.UrlTokenDecode(text)
                , _optionalEntropy, DataProtectionScope.LocalMachine));
        }
    }
}