using MMDExtensions.PMX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MMDExtensions
{
    public class PMXProvider
    {
        private SevenZipStock stock;

        private bool IsArchive(string path)
        {
            FileInfo fi = new(path);

            if (fi.Extension.ToLower() == ".zip" || fi.Extension.ToLower() == ".7z")
            {
                return true;
            }
            return false;
        }

        public void Load(string path)
        {
            if (IsArchive(path))
            {
                stock = new(path);
            }
        }

        public Stream[] GetPMX()
        {
            var found = stock.ContentDict.Where(pair => pair.Key.ToLower().EndsWith(".pmx"));

            if (found != null && found.Count() > 0)
            {
                List<Stream> ret = new();
                foreach (var f in found)
                {
                    ret.Add(f.Value);
                }
                return ret.ToArray();
            }
            return null;
        }

        public PMXFormat GetPMXFormat(string path)
        {
            if (IsArchive(path))
            {
                return PMXLoaderScript.Import(GetPMX()[0]);
            }
            return PMXLoaderScript.Import(path);
        }

        public ImageSource GetTexture(string folder, string texturePath)
        {
            string ext = new FileInfo(texturePath).Extension.ToLower();

            if (stock == null)
            {
                string path = Path.Combine(folder, texturePath);

                if (PfimxUtils.IsPfimx(ext))
                {
                    return PfimxUtils.ToBitmapImage(path);
                }
                return new BitmapImage(new Uri(path, UriKind.Relative));
            }
            else
            {
                if (PfimxUtils.IsPfimx(ext))
                {
                    return PfimxUtils.ToBitmapImage(stock.ContentDict[texturePath]);
                }
                return BitmapUtils.ToBitmapImage(stock.ContentDict[texturePath], ext);
            }
        }
    }
}
