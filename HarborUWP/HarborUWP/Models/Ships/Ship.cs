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

            if (TimeUntilDone.IsDone())
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

        public void Offload(Harbor harbor, int amount)
        {
            if (this is BulkCarrierShip)
            {
                BulkCarrierShip ship = (BulkCarrierShip)this;
                ship.Inventory -= amount;
            }
            else if (this is ContainerShip)
            {
                ContainerShip ship = (ContainerShip)this;
                int maxSizeContainer = ship.Containers[0].maxSize;
                ship.totalWeight = ship.totalWeight - amount;
                                double newAmountOfContainers = (double)ship.totalWeight / (double)maxSizeContainer;
                ship.Containers.Clear();
                // Create Containers based on amount of weight
                ship.CreateContainers(ship.totalWeight / maxSizeContainer,0, new Container(0,ContainerItemType.Vehicles,0).maxSize);
                // Create extra container with remainder weight
                int leftoverWeight = (int)(maxSizeContainer * (newAmountOfContainers - Math.Truncate(newAmountOfContainers)));
                if (leftoverWeight > 0)
                {
                    ship.CreateContainers(1, 0, leftoverWeight);
                }
            }
            else if (this is OilTankerShip)
            {
                OilTankerShip ship = (OilTankerShip)this;
                ship.Inventory -= amount;
            }
        }

        private void load(Harbor harbor, int amount)
        {
            if (this is BulkCarrierShip)
            {
                BulkCarrierShip ship = (BulkCarrierShip)this;
                ship.Inventory += amount;
            }
            else if (this is ContainerShip)
            {
                ContainerShip ship = (ContainerShip)this;
                int maxSizeContainer = ship.Containers[0].maxSize;
                ship.totalWeight = ship.totalWeight - amount;
                double newAmountOfContainers = (double)ship.totalWeight / (double)maxSizeContainer;
                // Create Containers based on amount of weight
                ship.CreateContainers(ship.totalWeight / maxSizeContainer, 0, new Container(0, ContainerItemType.Vehicles, 0).maxSize);
                // Create extra container with remainder weight
                int leftoverWeight = (int)(maxSizeContainer * (newAmountOfContainers - Math.Truncate(newAmountOfContainers)));
                 foreach (Container container in ship.Containers)
                {
                    if(container.curSize < 4000 && (container.curSize + leftoverWeight) < 4000)
                    {
                        container.curSize = container.curSize + leftoverWeight;
                        break;
                    }
                    else if(container.curSize < 4000 && (container.curSize + leftoverWeight) <= 4000)
                    {
                        leftoverWeight = leftoverWeight - container.curSize;
                        container.curSize = container.curSize + leftoverWeight;
                    }
                }
                if (leftoverWeight > 0)
                {
                    ship.CreateContainers(1, 0, leftoverWeight);
                }
            }
            else if (this is OilTankerShip)
            {
                OilTankerShip ship = (OilTankerShip)this;
                ship.Inventory += amount;
            }
        }
    }
}
