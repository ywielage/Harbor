using HarborUWP.Models.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborUWP.Models
{
    internal class DockingStation
    {
        private int Number { get; set; }
        private Ship Ship { get; set; }

        public DockingStation(int number)
        {
            Number = number;
            Ship = null;
        }

        public bool IsOccupied()
        {
            return Ship != null;
        }

        public void DockShip(Ship ship)
        {
            this.Ship = ship;
        }

        public void LeaveShip()
        {
            this.Ship = null;
        }
    }
}
