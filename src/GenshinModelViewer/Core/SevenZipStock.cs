using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace GenshinModelViewer.Core
{
    public class SevenZipStock : IDisposable
    {
        public Dictionary<string, Stream> ContentDict = new();

        public SevenZipStock(string path)
        {
            try
            {
                Dispose();
                ContentDict = SevenZipHelper.ArchiveStream(path, string.Empty);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void Dispose()
        {
            foreach (var pair in ContentDict)
            {
                try
                {
                    pair.Value.Dispose();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }
    }
}
