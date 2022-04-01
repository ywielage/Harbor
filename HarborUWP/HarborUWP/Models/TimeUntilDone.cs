using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace HarborUWP.Models
{
    internal class TimeUntilDone : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual Task OnPropertyChanged(string propertyName)
        {
            _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs(propertyName));
                    }
                }
            );

            return null;
        }
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        private int durationInMins;

        public int DurationInMins
        {
            get
            {
                return durationInMins;
            }
            set
            {
                if (value != durationInMins)
                {
                    SetField(ref durationInMins, value, "DurationInMins");
                }
            }
        }

        public TimeUntilDone(int durationInMins)
        {
            DurationInMins = durationInMins;
        }
    }
}
