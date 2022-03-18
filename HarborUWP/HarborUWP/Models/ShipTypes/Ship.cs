using HarborApp.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborApp.Models.ShipTypes
{
    internal abstract class Ship
    {
        public int Id { get; set; }
        public State State { get; set; }
        public TimeUntilDone TimeUntilDone { get; set; }
        protected int minPercantageCapacity = 80;
        protected int maxCapacity;

        protected Ship(int id, State state, int minPercantageCapacity, int maxCapacity)
        {
            Id = id;
            State = state;
            TimeUntilDone = null;
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

        public void OffLoad()
        {
            throw new NotImplementedException();
        }

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void Dock()
        {
            throw new NotImplementedException();
        }

        public void Leave()
        {
            throw new NotImplementedException();
        }
    }
}
