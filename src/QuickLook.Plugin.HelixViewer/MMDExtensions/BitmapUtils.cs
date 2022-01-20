using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MMDExtensions
{
    internal static class BitmapUtils
    {
        public static ImageSource ToBitmapImage(Stream stream, string ext)
        {
            Bitmap bitmap = new(stream);
            ImageFormat imageFormat = ext switch
            {
                ".png" => ImageFormat.Png,
                ".bmp" => ImageFormat.Bmp,
                ".emf" => ImageFormat.Emf,
                ".wmf" => ImageFormat.Wmf,
                ".gif" => ImageFormat.Gif,
                ".jpeg" => ImageFormat.Jpeg,
                ".jpg" => ImageFormat.Jpeg,
                ".tiff" => ImageFormat.Tiff,
                ".exif" => ImageFormat.Exif,
                ".ico" => ImageFormat.Icon,
                ".icon" => ImageFormat.Icon,
                _ => null,
            };
            BitmapImage bitmapImage = bitmap.ToBitmapImage(imageFormat);
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
    }
}
