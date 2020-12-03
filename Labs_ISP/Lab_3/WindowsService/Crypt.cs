using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace WindowsService1
{
    class Crypt
    {
        private static readonly AesCryptoServiceProvider aes = new AesCryptoServiceProvider();


        public static void Encrypt(Stream sourceStream, Stream targetStream)
        {
            using (var cryptoStream = new CryptoStream(targetStream, aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write))
            {
                sourceStream.CopyTo(cryptoStream);
            }
        }

        public static void Decrypt(Stream sourceStream, Stream targetStream)
        {
            using (var deCryptoStream = new CryptoStream(sourceStream, aes.CreateDecryptor(aes.Key, aes.IV), CryptoStreamMode.Read))
            {
                deCryptoStream.CopyTo(targetStream);
            }
        }

        public static string EncryptedFilePath(string filename, string targetPath)
        {
            filename = filename.Replace(Path.GetDirectoryName(filename), targetPath);
            return filename.Replace(Path.GetFileName(filename), Path.GetFileNameWithoutExtension(filename) + "_encrypt" + Path.GetExtension(filename));
        }

        public static string DecryptedFilePath(string filename, string targetPath)
        {
            filename = filename.Replace(Path.GetDirectoryName(filename), targetPath);
            return filename.Replace(Path.GetFileName(filename), Path.GetFileNameWithoutExtension(filename) + "_dencrypt" + Path.GetExtension(filename));
        }
    }
}
