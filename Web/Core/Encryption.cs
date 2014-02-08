using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Web.Core
{
    public class Encryption
    {

        private static readonly byte[] key = new byte[24] { 105, 238, 117, 250, 128, 6, 140, 18, 151, 29, 162, 41, 174, 52, 185, 64, 197, 75, 208, 86, 220, 98, 231, 109 };
        private static readonly byte[] iVector = new byte[8] { 82, 216, 94, 227, 105, 238, 117, 250 };

        public static string Encrypt(string inputString)
        {
            if (inputString != null)
            {
                byte[] buffer = Encoding.ASCII.GetBytes(inputString);
                TripleDESCryptoServiceProvider tripleDESProvider = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();
                tripleDESProvider.Key = key;
                tripleDESProvider.IV = iVector;
                ICryptoTransform iTransform = tripleDESProvider.CreateEncryptor();
                return Convert.ToBase64String(iTransform.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            else
                return null;
        }

        public static string Decrypt(string inputString)
        {
            if (inputString != null)
            {
                byte[] buffer = Convert.FromBase64String(inputString);
                TripleDESCryptoServiceProvider tripleDESProvider = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();
                tripleDESProvider.Key = key;
                tripleDESProvider.IV = iVector;
                ICryptoTransform iTransform = tripleDESProvider.CreateDecryptor();
                return Encoding.ASCII.GetString(iTransform.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            else
                return null;
        }
    }
}