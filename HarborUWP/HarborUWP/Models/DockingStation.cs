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
        public int Number { get; set; }
        public Ship Ship { get; set; }

        public DockingStation(int number)
        {
            Number = number;
            Ship = null;
        }

        public bool IsOccupied()
        {
            return Ship != null;
        }
    }
}
