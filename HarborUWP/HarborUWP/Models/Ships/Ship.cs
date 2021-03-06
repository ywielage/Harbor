using HarborUWP.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace HarborUWP.Models.Ships
{
    internal abstract class Ship : INotifyPropertyChanged
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

        private State state;
        private TimeUntilDone timeUntilDone;

        public int Id { get; set; }
        public ShipTypes ShipType { get; set; }
        public State State
        {
            get
            {
                return state;
            }
            set
            {
                if (value != state)
                {
                    SetField(ref state, value, "State");
                }
            }
        }
        public TimeUntilDone TimeUntilDone
        {
            get
            {
                return timeUntilDone;
            }
            set
            {
                if (!value.Equals(timeUntilDone))
                {
                    SetField(ref timeUntilDone, value, "TimeUntilDone");
                }
            }
        }
        private int minPercentageCapacity;
        private int maxCapacity;

        protected Ship(int id, int minPercentageCapacity, int maxCapacity)
        {
            Id = id;
            State = State.InOpenWaters;
            SetNewTimeUntilDone(10, 100);
            this.minPercentageCapacity = minPercentageCapacity;
            this.maxCapacity = maxCapacity;
        }

        public int GetMinPercentageCapacity()
        {
            return minPercentageCapacity;
        }

        public int GetMaxCapacity()
        {
            return maxCapacity;
        }

        private TimeUntilDone SetNewTimeUntilDone(int minDuration, int maxDuration)
        {
            Random random = new Random();
            int duration = random.Next(minDuration, maxDuration + 1);
            return TimeUntilDone = new TimeUntilDone(duration);
        }

        public void Update()
        {
            if (TimeUntilDone.DurationInMins != 0)
            {
                TimeUntilDone.DurationInMins--;
            }

            if (TimeUntilDone.DurationInMins <= 0)
            {
                switch (State)
                {
                    case State.InOpenWaters:
                        State = State.WaitingInPortWaters;
                        TimeUntilDone = SetNewTimeUntilDone(1, 15);
                        break;
                    case State.WaitingInPortWaters:
                        State = State.Docking;
                        TimeUntilDone = SetNewTimeUntilDone(1, 5);
                        break;
                    case State.Docking:
                        State = State.Offloading;
                        TimeUntilDone = SetNewTimeUntilDone(15, 40);
                        break;
                    case State.Offloading:
                        State = State.Loading;
                        TimeUntilDone = SetNewTimeUntilDone(15, 40);
                        break;
                    case State.Loading:
                        State = State.Leaving;
                        TimeUntilDone = SetNewTimeUntilDone(1, 5);
                        break;
                    case State.Leaving:
                        State = State.InOpenWaters;
                        TimeUntilDone = SetNewTimeUntilDone(5, 20);
                        break;
                }
            }
        }

        public static ShipTypes GenerateRandomShipType()
        {
            Random random = new Random();
            int shipTypeNumber = random.Next(Enum.GetNames(typeof(ShipTypes)).Length);

            switch (shipTypeNumber)
            {
                case 0:
                default:
                    return ShipTypes.OilTanker;
                case 1:
                    return ShipTypes.Container;
                case 2:
                    return ShipTypes.BulkCarrier;
            }
        }

        public abstract void OffLoad(Harbor harbor);

        public abstract void Load(Harbor harbor);
    }
}