using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GenshinModelViewer.Core
{
    public class PMXProvider
    {
        private SevenZipStock? stock;
        private string path = null;

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

        public Stream GetPMX(Func<string[], string> selector)
        {
            string[] pmxs = stock.ContentDict?.Keys.Where(k => k.ToLower().EndsWith(".pmx")).ToArray();

            if (pmxs == null || pmxs.Length <= 0)
            {
                return null;
            }

            path = selector.Invoke(pmxs);
            return stock.ContentDict[path];
        }

        public PMXFormat GetPMXFormat(string path, Func<string[], string> selector = null)
        {
            if (IsArchive(path))
            {
                Stream pmxStream = GetPMX(selector ?? (ss => ss.First()));

                if (pmxStream == null)
                {
                    return null;
                }
                return PMXLoaderScript.Import(pmxStream);
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
                string GetRelativePath(string relativeFrom, string path)
                {
                    string[] pathSplitted = path.Replace('/', '\\').ToLower().Split('\\');

                    if (pathSplitted.Length >= 2)
                    {
                        string[] pathNew = new string[pathSplitted.Length - 1];

                        Array.Copy(pathSplitted, pathNew, pathSplitted.Length - 1);

                        return $"{string.Join("\\", pathNew)}\\{relativeFrom.Replace('/', '\\').ToLower()}";
                    }
                    else
                    {
                        return relativeFrom.Replace('/', '\\').ToLower();
                    }
                }

                string key = GetRelativePath(texturePath, path);

                Debug.WriteLine($"[LoadKey] {key}");
                Stream image = stock.ContentDict[key];

                if (PfimxUtils.IsPfimx(ext))
                {
                    return PfimxUtils.ToBitmapImage(image);
                }
                return BitmapUtils.ToBitmapImage(image);
            }
        }
    }
}
