using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PhysicManagement.Common
{
    public class Cryptography
    {
        public static string Encrypt(string input)
        {
            string UV = "Adsw_dasd";
            return Encrypt(input, UV);
        }
        public static string EncryptByUV(string input, string uv)
        {
            try
            {
                return Encrypt(input, uv);
            }
            catch {
                return "";
            }
        }
        public static string Decrypt(string input)
        {
            string UV = "Adsw_dasd";
            return Decrypt(input, UV);
        }
        public static bool TryDecrypt(string input, out string output)
        {
            try
            {
                output = Decrypt(input);
                return true;
            }
            catch
            {
                output = input;
                return false;
            }
        }
        public static bool TryDecrypt(string input, string uv, out string output)
        {
            try
            {
                output = Decrypt(input, uv);
                return true;
            }
            catch
            {
                output = input;
                return false;
            }
        }
        private static byte[] Encrypt(byte[] clearData, byte[] key, byte[] iv)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = key;
            alg.IV = iv;
            CryptoStream cs = new CryptoStream(ms,
               alg.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(clearData, 0, clearData.Length);
            cs.Close();
            byte[] encryptedData = ms.ToArray();
            return encryptedData;
        }
        private static string Encrypt(string clearText, string password)
        {
            byte[] clearBytes =
              Encoding.Unicode.GetBytes(clearText);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password,
                new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return Convert.ToBase64String(encryptedData);

        }

        private static byte[] Decrypt(byte[] cipherData, byte[] key, byte[] iv)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = key;
            alg.IV = iv;
            CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(cipherData, 0, cipherData.Length);
            cs.Close();
            byte[] decryptedData = ms.ToArray();
            return decryptedData;
        }
        public static string Decrypt(string cipherText, string password)
        {

            byte[] cipherBytes = Convert.FromBase64String(cipherText.Replace(" ", "+"));
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return Encoding.Unicode.GetString(decryptedData);
        }
    }
}
