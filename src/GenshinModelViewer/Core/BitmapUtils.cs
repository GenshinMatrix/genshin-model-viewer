using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Bitmap = System.Drawing.Bitmap;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace GenshinModelViewer
{
    internal static class BitmapUtils
    {
        public static ImageSource ToBitmapImage(Stream stream)
        {
            BitmapImage bitmapImage = new();

            stream.Position = 0;
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();
            bitmapImage.Freeze();
            return bitmapImage;
        }

        public static BitmapImage ToBitmapImage(this Bitmap bitmap, ImageFormat imageFormat = null)
        {
            MemoryStream stream = new();
            bitmap.Save(stream, imageFormat ?? ImageFormat.Png);
            BitmapImage image = new();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }

        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            using MemoryStream memory = new();
            bitmap.Save(memory, ImageFormat.Png);
            memory.Position = 0;

            BitmapImage bitmapImage = new();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            return bitmapImage;
        }

        public static Bitmap ToBitmap(this BitmapImage bitmapImage)
        {
            using MemoryStream outStream = new();
            BmpBitmapEncoder enc = new();
            enc.Frames.Add(BitmapFrame.Create(bitmapImage));
            enc.Save(outStream);
            Bitmap bitmap = new(outStream);

            return bitmap;
        }

        public static void Save(this ImageSource imageSource, string fileName)
        {
            (imageSource as BitmapSource)?.Save(fileName);
        }

        public static void Save(this BitmapSource bitmapSource, string fileName)
        {
            PngBitmapEncoder encoder = new();

            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

            string dir = new FileInfo(fileName).DirectoryName;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using Stream stream = File.Create(fileName);

            encoder.Save(stream);
        }
    }
}
