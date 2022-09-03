using SevenZip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Resources;

namespace GenshinModelViewer
{
    public class SevenZipHelper
    {
        public static Func<string, string> ModifyArchiveKey = (key) => key.Replace('/', '\\').ToLower();

        static SevenZipHelper()
        {
            string lib7zPath = $@"{Environment.CurrentDirectory}\7z.dll";

            if (!File.Exists(lib7zPath))
            {
                byte[] lib7z = GetBytes("pack://application:,,,/GenshinModelViewer;component/Resources/7z.dll");

                try
                {
                    File.WriteAllBytes(lib7zPath, lib7z);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            SetLibraryPath(lib7zPath);
        }

        public static byte[] GetBytes(string uriString)
        {
            Uri uri = new(uriString);
            StreamResourceInfo info = Application.GetResourceStream(uri);
            using BinaryReader stream = new(info.Stream);
            return stream.ReadBytes((int)info.Stream.Length);
        }

        public static Dictionary<string, Stream> ArchiveStream(string archiveFullName, string? password = null)
        {
            Dictionary<string, Stream> archiveDict = new();
            using SevenZipExtractor sevenZipExtrator = string.IsNullOrEmpty(password) ? new(archiveFullName) : new(archiveFullName, password);

            void ReadFile(string fileName)
            {
                MemoryStream stream = new();

                sevenZipExtrator.ExtractFile(fileName, stream);
                stream.Seek(0, SeekOrigin.Begin);

                archiveDict.Add(ModifyArchiveKey(fileName), stream);
            }

            foreach (ArchiveFileInfo info in sevenZipExtrator?.ArchiveFileData!)
            {
                if (!info.IsDirectory)
                {
                    ReadFile(info.FileName);
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
