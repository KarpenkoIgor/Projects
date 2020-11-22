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
        private readonly int[] _key;
        private readonly int[] _inversedKey;

        public Crypt(string key)
        {
            var indexPairs = key.Select((chr, idx1) => new { chr, idx1 }) .OrderBy(arg => arg.chr)
                .Select((arg, idx2) =>
                    new
                    {
                        arg.idx1,
                        idx2
                    })
                .ToArray();

            _key = indexPairs
                .OrderBy(arg => arg.idx1)
                .Select(arg => arg.idx2)
                .ToArray();

            _inversedKey = indexPairs
                .OrderBy(arg => arg.idx2)
                .Select(arg => arg.idx1)
                .ToArray();
        }

        public void Encrypt(string sourceFile, string destinationFile)
        {
            EncryptDecrypt(sourceFile, destinationFile, _key);
        }

        public void Decrypt(string sourceFile, string destinationFile)
        {
            EncryptDecrypt(sourceFile, destinationFile, _inversedKey);
        }

        private static void EncryptDecrypt(string sourceFile, string destinationFile, int[] key)
        {
            var keyLength = key.Length;
            var buffer1 = new byte[keyLength];
            var buffer2 = new byte[keyLength];
            using (var source = new FileStream(sourceFile, FileMode.Open))
            using (var destination = new FileStream(destinationFile, FileMode.OpenOrCreate))
            {
                while (true)
                {
                    var read = source.Read(buffer1, 0, keyLength);
                    if (read == 0)
                    {
                        return;
                    }
                    else if (read < keyLength)
                    {
                        for (int i = read; i < keyLength; i++)
                        {
                            buffer1[i] = (byte)' ';
                        }
                    }

                    for (var i = 0; i < keyLength; i++)
                    {
                        var idx = i / keyLength * keyLength + key[i % keyLength];
                        buffer2[idx] = buffer1[i];
                    }

                    destination.Write(buffer2, 0, keyLength);
                }
            }
        }
    }
}
