using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace WindowsService1
{
    public class Settings
    {
        public string SourcePath { get; set;}
        public string TargetPath { get; set;}
        public ArchiveSetting ArchiveSetting { get; set;}
    }
    public class ArchiveSetting 
    {
        public bool NeedToArchive { get; set;}
        public bool NeedToEncrypt { get; set;}
        public string CompressingLevel { get; set;}
    }

}
