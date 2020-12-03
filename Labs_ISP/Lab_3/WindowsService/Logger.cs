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
            Settings settings;
            string sourcePath;
            string targetPath;
            bool needToArchive;
            bool needToEncrypt;


            public Logger()
            {      
                ConfigurationSettings confSettings = new ConfigurationSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.xml"));          
                settings = confSettings.GetSetting<Settings>();             
                if (settings.SourcePath != null) this.sourcePath = settings.SourcePath;
                if (settings.TargetPath != null) this.targetPath = settings.TargetPath;
                this.needToArchive = settings.ArchiveSetting.NeedToArchive;
                this.needToEncrypt = settings.ArchiveSetting.NeedToEncrypt;
        
            watcher = new FileSystemWatcher(sourcePath);
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
                string gzfile = Path.ChangeExtension(e.Name, "gz");
                string tocomp = Path.Combine(targetPath, "archive", gzfile);
                if (needToArchive)
                {  
                    Archiver.Compress(e.FullPath, tocomp, needToEncrypt);
                    Archiver.Decompress(tocomp, Path.ChangeExtension(tocomp, "txt"), needToEncrypt);
                }
            }

            private void Watcher_Changed(object sender, FileSystemEventArgs e)
            {
                string fileEvent = "изменен";
                string filePath = e.FullPath;
                RecordEntry(fileEvent, filePath);
                string gzfile = Path.ChangeExtension(e.Name, "gz");
                string tocomp = Path.Combine(targetPath, "archive", gzfile);
                if (needToArchive)
                {
                    Archiver.Compress(e.FullPath, tocomp, needToEncrypt);
                    Archiver.Decompress(tocomp, Path.ChangeExtension(tocomp, "txt"), needToEncrypt);
                }
            }
            private void Watcher_Created(object sender, FileSystemEventArgs e)
            {
                string fileEvent = "создан";
                string filePath = e.FullPath;
                RecordEntry(fileEvent, filePath);
                string gzfile = Path.ChangeExtension(e.Name, "gz");
                string tocomp = Path.Combine(targetPath,"archive", gzfile);
                if (needToArchive)
                {
                    Archiver.Compress(e.FullPath, tocomp, needToEncrypt);
                    Archiver.Decompress(tocomp, Path.ChangeExtension(tocomp, "txt"), needToEncrypt);
                }
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
                    using (StreamWriter writer = new StreamWriter(Path.Combine(targetPath, "templog.txt"), true))
                    {
                        writer.WriteLine(String.Format("{0} файл {1} был {2}",
                            DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), filePath, fileEvent));
                        writer.Flush();
                    }
                }
            }
           public string DataPath()
           {
                 return DateTime.Now.ToString("yyyy\\MM\\dd");
           }
    }
}
