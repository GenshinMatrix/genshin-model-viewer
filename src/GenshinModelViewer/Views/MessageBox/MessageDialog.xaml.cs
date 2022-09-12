using ModernWpf.Controls;
using System.Windows;

namespace Model.Viewer.Plugin
{
    public partial class MessageDialog : ContentDialog
    {
        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(ModelSelectionDialog), new PropertyMetadata(null!));

        public MessageDialog(string title, string message)
        {
            Message = message;
            DataContext = this;
            InitializeComponent();
            Title = title;
        }
    }
}
