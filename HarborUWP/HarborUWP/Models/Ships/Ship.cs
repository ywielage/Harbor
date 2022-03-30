using HarborUWP.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HarborUWP.Models.Ships
{
    internal abstract class Ship
    {
        public int Id { get; set; }
        public State State { get; set; }
        public TimeUntilDone TimeUntilDone { get; set; }
        protected int minPercantageCapacity = 80;
        protected int maxCapacity;

        protected Ship(int id, int minPercantageCapacity, int maxCapacity)
        {
            Id = id;
            State = State.InOpenWaters;
            SetNewTimeUntilDone(1, 15);
            this.minPercantageCapacity = minPercantageCapacity;
            this.maxCapacity = maxCapacity;
        }

        public int GetMaxPercentageCapacity()
        {
            return minPercantageCapacity;
        }

        public int GetMaxCapacity()
        {
            return maxCapacity;
        }

        private void SetNewTimeUntilDone(int minDuration, int maxDuration) 
        {
            Random random = new Random();
            int duration = random.Next(minDuration, maxDuration + 1);
            TimeUntilDone = new TimeUntilDone(duration);
            
        }

        public string Update() 
        {
            TimeUntilDone.Update();

            if (TimeUntilDone.IsDone())
            {
                switch (State)
                {
                    case State.InOpenWaters:
                        State = State.WaitingInPortWaters;
                        SetNewTimeUntilDone(1,15);
                        break;
                    case State.WaitingInPortWaters:
                        //TODO: check if DockingStation is available yes?--> set state Docking, no? --> set duration again
                        State = State.Docking;
                        SetNewTimeUntilDone(1,5);
                        break;
                    case State.Docking:
                        State = State.Offloading;
                        //TODO: start offloading
                        SetNewTimeUntilDone(15,40);
                        break;
                    case State.Offloading:
                        State = State.Loading;
                        //TODO: start loading
                        SetNewTimeUntilDone(15,40);
                        break;
                    case State.Loading:
                        State = State.Leaving;
                        SetNewTimeUntilDone(1,5);
                        break;
                    case State.Leaving:
                        State = State.InOpenWaters;
                        SetNewTimeUntilDone(5,20);
                        break;
                }
            }
            return $"Ship {Id} is now {StatusConverter(State)}, for another {TimeUntilDone.DurationInMins} minutes";
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
        public string StatusConverter(State state)
        {
            switch (state)
            {
                case State.InOpenWaters:
                    return "in open waters";
                case State.WaitingInPortWaters:
                    return "waiting in port waters";
                default: return State.ToString().ToLower();
            }
        }

/*        public abstract void OffLoad(Harbor harbor, int amount);
        // TODO: String returnen
        public abstract void Load(Harbor harbor, int amount);*/
    }
}
