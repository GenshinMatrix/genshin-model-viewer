using ModernWpf.Controls;
using System.Windows;
using System;
using System.Threading.Tasks;

namespace Model.Viewer.Plugin
{
    public partial class ModelSelectionDialog : ContentDialog
    {
        public class ModelSelection
        {
            public string Text { get; set; }
            public override string ToString() => Text;
        }

        public ModelSelection[] Texts
        {
            get => (ModelSelection[])GetValue(TextsProperty);
            set => SetValue(TextsProperty, value);
        }
        public static readonly DependencyProperty TextsProperty = DependencyProperty.Register("Texts", typeof(ModelSelection[]), typeof(ModelSelectionDialog), new PropertyMetadata(null!));

        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(ModelSelectionDialog), new PropertyMetadata(0));

        public ModelSelectionDialog(string[] texts)
        {
            Texts = Array.ConvertAll(texts, m => new ModelSelection() { Text = m });
            DataContext = this;
            InitializeComponent();
        }

        public async Task<string> GetSelectedAsync()
        {
            await ShowAsync();
            return Texts[SelectedIndex].ToString();
        }
    }
}
