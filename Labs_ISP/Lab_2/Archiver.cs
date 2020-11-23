using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace WindowsService1
{
    class Archiver
    {
        public static void Compress(string sourceFile, string compressedFile)
        {
            try
            {
                string encrFileName = Crypt.EncryptedFilePath(sourceFile, "D:\\Target\\crypt");
                using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
                {
                    using (FileStream targetStream = File.Create(compressedFile))
                    {
                        using (FileStream encryptStream = File.Create(encrFileName))
                        {
                            Crypt.Encrypt(sourceStream, encryptStream);
                            using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                            {
                                using (FileStream finalStream = new FileStream(encrFileName, FileMode.OpenOrCreate))
                                {
                                    finalStream.CopyTo(compressionStream);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter("D:\\Target\\sourcelog.txt", true))
                {
                    writer.WriteLine("Ошибка архивации данных:{0}", ex.Message);
                    writer.Flush();
                }
            }
        }

        public static void Decompress(string compressedFile, string targetFile)
        {
            try
            {
                string decrFileName = Crypt.DecryptedFilePath(compressedFile, "D:\\Target\\crypt");
                using (FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate))
                {
                    using (FileStream targetStream = File.Create(targetFile))
                    {
                        using (FileStream decryptStream = File.Create(decrFileName))
                        {
                            using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                            {
                                decompressionStream.CopyTo(decryptStream);
                            }
 
                        }
                        using (FileStream finaleStream = new FileStream(decrFileName, FileMode.OpenOrCreate))
                        {
                            Crypt.Decrypt(finaleStream, targetStream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter("D:\\Target\\sourcelog.txt", true))
                {
                    writer.WriteLine("Ошибка деархивации данных:{0}", ex.Message);
                    writer.Flush();
                }
            }

        }
    }

   
}
