using Microsoft.Win32;
using QuickLook.Plugin.HelixViewer;
using SharpVectors.Converters;
using System;
using System.Windows;
using System.Windows.Media;

namespace Genshin.ModelViewer
{
    public partial class MainWindow : Window
    {
        public string ModelPath
        {
            get { return (string)GetValue(ModelPathProperty); }
            set { SetValue(ModelPathProperty, value); }
        }

        public static readonly DependencyProperty ModelPathProperty = DependencyProperty.Register("ModelPath", typeof(string), typeof(MainWindow), new PropertyMetadata("", OnModelPathChanged));

        public MainWindow()
        {
            InitializeComponent();
            AllowDrop = true;

            Drop += (s, e) =>
            {
                try
                {
                    string fileName = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();

                    LoadModel(fileName);
                }
                catch
                {
                }
            };

            gridOpen.MouseEnter += (s, e) =>
            {
                Brush brush = new SolidColorBrush(Color.FromRgb(107, 107, 107));
                borderOpen.BorderBrush = brush;

                textBlockOpen.Foreground = brush;
                for (int i = 1; i <= 7; i++)
                {
                    var svg = FindName($"svgc{i}") as SvgViewbox;

                    foreach (DrawingGroup d in svg.Drawings.Children)
                    {
                        foreach (GeometryDrawing dd in d.Children)
                        {
                            dd.Brush = brush;
                        }
                    }
                }
            };
            gridOpen.MouseLeave += (s, e) =>
            {
                Brush brush = new SolidColorBrush(Color.FromRgb(170, 170, 170));
                borderOpen.BorderBrush = brush;

                textBlockOpen.Foreground = brush;
                for (int i = 1; i <= 7; i++)
                {
                    var svg = FindName($"svgc{i}") as SvgViewbox;

                    foreach (DrawingGroup d in svg.Drawings.Children)
                    {
                        foreach (GeometryDrawing dd in d.Children)
                        {
                            dd.Brush = brush;
                        }
                    }
                }
            };
            gridOpen.MouseLeftButtonUp += (s, e) =>
            {
                var dialog = new OpenFileDialog()
                {
                    Title = "选择模型文件",
                    Filter = "模型(*.pmx,*.zip,*.7z)|*.pmx;*.zip;*.7z",
                    RestoreDirectory = true,
                    DefaultExt = "pmx",
                };
                if (dialog.ShowDialog() ?? false)
                {
                    LoadModel(dialog.FileName);
                }
            };
        }

        private static void OnModelPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MainWindow self)
            {
                self.LoadModel(e.NewValue as string);
            }
        }

        public void LoadModel(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                HelixViewerPanel render = new(path);

                gridRoot.Children.Clear();
                gridRoot.Children.Add(render);
            }
        }
    }
}
