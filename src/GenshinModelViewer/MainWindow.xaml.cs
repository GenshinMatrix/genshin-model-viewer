using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GenshinModelViewer
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
                Brush brush = new SolidColorBrush(Color.FromArgb(255, 107, 107, 107));
                Dictionary<DependencyProperty, Brush> dic = new()
                {
                    [TextBlock.ForegroundProperty] = brush,
                    [Border.BorderBrushProperty] = brush,
                };
                gridOpen.Children.ForEachDeep<TextBlock>(textBlock =>
                {
                    StoryboardUtils.BeginBrushStoryboard(textBlock, dic);
                });
                StoryboardUtils.BeginBrushStoryboard(borderOpen, dic);
            };
            gridOpen.MouseLeave += (s, e) =>
            {
                Brush brush = new SolidColorBrush(Color.FromArgb(170, 170, 170, 170));
                Dictionary<DependencyProperty, Brush> dic = new()
                {
                    [TextBlock.ForegroundProperty] = brush,
                    [Border.BorderBrushProperty] = brush,
                };
                borderOpen.BorderBrush = brush;

                gridOpen.Children.ForEachDeep<TextBlock>(textBlock =>
                {
                    StoryboardUtils.BeginBrushStoryboard(textBlock, dic);
                });
                StoryboardUtils.BeginBrushStoryboard(borderOpen, dic);
            };

            gridOpen.MouseLeftButtonUp += (s, e) =>
            {
                var dialog = new OpenFileDialog()
                {
                    Title = "Select DMM Model",
                    Filter = "DMM Model(*.pmx,*.zip,*.7z)|*.pmx;*.zip;*.7z",
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
                render.ModelPath = path;
                render.Visibility = Visibility.Visible;
                gridOpen.Visibility = Visibility.Collapsed;
            }
        }
    }
}
