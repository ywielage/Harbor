using HarborUWP.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            TimeUntilDone = SetNewTimeUntilDone();
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

        public void UpdateTimeUntilDone()
        {
            throw new NotImplementedException();
        }

        private TimeUntilDone SetNewTimeUntilDone() 
        {
            Random random = new Random();
            int duration = random.Next(1, 4);
            TimeUntilDone td = new TimeUntilDone(duration);
            return td;
        }

        public string Update() 
        {
            TimeUntilDone.Update();

            if (TimeUntilDone.DurationInMins == 0)
            {
                TimeUntilDone = SetNewTimeUntilDone();

                switch (State)
                {
                    case State.InOpenWaters:
                        State = State.WaitingInPortWaters;
                        break;
                    case State.WaitingInPortWaters:
                        State = State.Docking;
                        break;
                    case State.Docking:
                        State = State.Offloading;
                        break;
                    case State.Loading:
                        State = State.Leaving;
                        break;
                    case State.Offloading:
                        State = State.Loading;
                        break;
                    case State.Leaving:
                        State = State.InOpenWaters;
                        break;
                }
            }
            return $"Ship {Id} is now {State.ToString().ToLower()}, for another {TimeUntilDone.DurationInMins} minutes";
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
    }
}
