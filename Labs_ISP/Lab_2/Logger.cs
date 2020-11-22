using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace WindowsService1
{
    class Logger
    {
      
            FileSystemWatcher watcher;
            object obj = new object();
            bool enabled = true;
            public Logger()
            {
                watcher = new FileSystemWatcher("D:\\Source");
                watcher.Deleted += Watcher_Deleted;
                watcher.Created += Watcher_Created;
                watcher.Changed += Watcher_Changed;
                watcher.Renamed += Watcher_Renamed;
            }

            public void Start()
            {
                watcher.EnableRaisingEvents = true;
                while (enabled)
                {
                    Thread.Sleep(1000);
                }
            }
            public void Stop()
            {
                watcher.EnableRaisingEvents = false;
                enabled = false;
            }
  
            private void Watcher_Renamed(object sender, RenamedEventArgs e)
            {
                string fileEvent = "переименован в " + e.FullPath;
                string filePath = e.OldFullPath;
                RecordEntry(fileEvent, filePath);
                string gzfile = Extension(e.Name) + "gz";
                string tocomp = "D:\\Target\\archive\\" + gzfile;
                Archiver.Compress(e.FullPath, tocomp);
                Archiver.Decompress(tocomp, Extension(tocomp) + "txt");
            }

            private void Watcher_Changed(object sender, FileSystemEventArgs e)
            {
                string fileEvent = "изменен";
                string filePath = e.FullPath;
                RecordEntry(fileEvent, filePath);
                string gzfile = Extension(e.Name) + "gz";
                string tocomp = "D:\\Target\\archive\\" + gzfile;
                Archiver.Compress(e.FullPath, tocomp);
                Archiver.Decompress(tocomp, Extension(tocomp) + "txt");
            }
            private void Watcher_Created(object sender, FileSystemEventArgs e)
            {
                string fileEvent = "создан";
                string filePath = e.FullPath;
                RecordEntry(fileEvent, filePath);
                string gzfile = Extension(e.Name) + "gz";
                string tocomp = "D:\\Target\\archive\\" + gzfile;
                Archiver.Compress(e.FullPath, tocomp);
                Archiver.Decompress(tocomp, Extension(tocomp) + "txt");
            }

            private void Watcher_Deleted(object sender, FileSystemEventArgs e)
            {
                string fileEvent = "удален";
                string filePath = e.FullPath;
                RecordEntry(fileEvent, filePath);
            }

            private void RecordEntry(string fileEvent, string filePath)
            {
                lock (obj)
                {
                    using (StreamWriter writer = new StreamWriter(@"D:\Target\templog.txt", true))
                    {
                        writer.WriteLine(String.Format("{0} файл {1} был {2}",
                            DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), filePath, fileEvent));
                        writer.Flush();
                    }
                }
            }
            public string Extension(string source)
            {
                 int i = source.Length - 1;
                 while (source[i - 1] != '.')
                     i--;
                 return source.Remove(i);
            }
    }
}
