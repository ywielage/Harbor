using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Core;

namespace HarborUWP.Models
{
    internal class WarehouseItemViewModel
    {
        private string name;
        private StorageObject storageObject;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
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
            OnPropertyChanged(propertyName);
            return true;
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                {
                    SetField(ref name, value, "Name");
                }
            }
        }

        public StorageObject StorageObject
        {
            get { return storageObject; }
            set
            {
                if (value != storageObject)
                {
                    SetField(ref storageObject, value, "Amount");
                }
            }
        }
    }
}
