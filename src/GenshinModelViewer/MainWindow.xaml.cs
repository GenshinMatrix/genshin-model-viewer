using GenshinModelViewer.Models;
using Microsoft.Win32;
using Model.Viewer.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GenshinModelViewer
{
    public partial class MainWindow : Window
    {
        public string ModelPath
        {
            get => (string)GetValue(ModelPathProperty);
            set => SetValue(ModelPathProperty, value);
        }
        public static readonly DependencyProperty ModelPathProperty = DependencyProperty.Register("ModelPath", typeof(string), typeof(MainWindow), new PropertyMetadata(null, (d, e) =>
        {
            if (d is MainWindow self)
                self.LoadModel(e.NewValue as string);
        }));

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
                OpenFromDialog();
            };

            KeyDown += (s, e) =>
            {
                switch (e.Key)
                {
                    case Key.F1:
                        OpenFromDialog();
                        break;
                    case Key.F11:
                        this.SetFullScreen();
                        break;
                    case Key.Escape:
                        this.SetFullScreen(true);
                        break;
                    case Key.R:
                        ResetCamera();
                        break;
                }
            };

            viewer.Selector = async ss =>
            {
                string selected = await new ModelSelectionDialog(ss).GetSelectedAsync();

                if (string.IsNullOrEmpty(selected))
                {
                    CancelModel();
                    return null;
                }
                return selected;
            };
        }

        public void LoadModel(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                viewer.ModelPath = path;
                viewer.Visibility = Visibility.Visible;
                gridOpen.Visibility = Visibility.Collapsed;
            }
        }

        public void CancelModel()
        {
            viewer.CancelLoadModel();
            viewer.Visibility = Visibility.Collapsed;
            gridOpen.Visibility = Visibility.Visible;
        }

        private void OpenFromDialog()
        {
            OpenFileDialog dialog = new()
            {
                Title = "Select Model",
                Filter = "DMM(*.pmx,*.zip,*.7z,*.rar)|*.pmx;*.zip;*.7z;*.rar",
                RestoreDirectory = true,
                DefaultExt = "pmx",
                InitialDirectory = Directory.Exists(ForDispatcher.ApplicationModelPath) ? ForDispatcher.ApplicationModelPath : null,
            };
            if (dialog.ShowDialog() ?? false)
            {
                LoadModel(dialog.FileName);
            }
        }

        private void ResetCamera()
        {
            viewer.ResetCamera();
        }
    }
}
