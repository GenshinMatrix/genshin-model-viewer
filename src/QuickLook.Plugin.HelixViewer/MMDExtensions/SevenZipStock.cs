using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace MMDExtensions
{
    public class SevenZipStock
    {
        public Dictionary<string, Stream> ContentDict = null;

        public SevenZipStock(string path)
        {
            try
            {
                ContentDict = SevenZipHelper.ArchiveStream(path, string.Empty);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
