using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace HarborUWP.Models
{
    public class StorageObject : INotifyPropertyChanged
    {
        private int amount;

        public int Amount 
        {
            get
            {
                return amount;
            }
            set
            {
                if (value != amount)
                {
                    SetField(ref amount, value, "Amount");
                }
            }
        }

        public StorageObject(int amount)
        {
            Amount = amount;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual async Task OnPropertyChangedAsync(string propertyName)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            }
            );
        }
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChangedAsync(propertyName);
            return true;
        }
    }
}
