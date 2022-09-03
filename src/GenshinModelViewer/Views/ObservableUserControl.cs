using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace GenshinModelViewer
{
    public class ObservableUserControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void Set<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            field = newValue;
            RaisePropertyChanged(propertyName);
        }

        protected virtual void Set2<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                RaisePropertyChanged(propertyName);
            }
        }

        protected virtual void RaisePropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
