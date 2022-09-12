using System;
using System.IO;

namespace GenshinModelViewer.Models
{
    public class ForDispatcher
    {
        public static string ApplicationModelPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"GenshinModelViewer\models");
    }
}
