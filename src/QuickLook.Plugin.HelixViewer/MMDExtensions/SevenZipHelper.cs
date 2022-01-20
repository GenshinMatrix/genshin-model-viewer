using SevenZip;
using System;
using System.Collections.Generic;
using System.IO;

namespace MMDExtensions
{
    public class SevenZipHelper
    {
        public static Func<string, string> ModifyArchiveKey = (key) => key;

        static SevenZipHelper()
        {
#if false
            string tempPath = $@"{Path.GetTempPath()}{nameof(SevenZipHelper)}";
            string _7zPath = $"{tempPath}\\SevenZip.dll";
            byte[] _7z = Resource._7z;

            Directory.CreateDirectory(tempPath);
            File.WriteAllBytes(_7zPath, _7z);
            SetLibraryPath(_7zPath);
#else
            SetLibraryPath($@"{Environment.CurrentDirectory}\SevenZip.dll");
#endif
        }

        public static Dictionary<string, Stream> ArchiveStream(string archiveFullName, string password = null)
        {
            Dictionary<string, Stream> archiveDict = new();
            using SevenZipExtractor sevenZipExtrator = string.IsNullOrEmpty(password) ? new(archiveFullName) : new(archiveFullName, password);

            foreach (ArchiveFileInfo info in sevenZipExtrator?.ArchiveFileData)
            {
                if (!info.IsDirectory)
                {
                    MemoryStream stream = new();

                    sevenZipExtrator.ExtractFile(info.FileName, stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    archiveDict.Add(ModifyArchiveKey(info.FileName), stream);
                }
            }
            return archiveDict;
        }

        public static void SetLibraryPath(string libraryPath)
        {
            SevenZipBase.SetLibraryPath(libraryPath);
        }
    }
}
