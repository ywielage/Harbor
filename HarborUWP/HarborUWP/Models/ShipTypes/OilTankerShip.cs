using HarborUWP.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborUWP.Models.ShipTypes
{
    internal class OilTankerShip : Ship
    {
        public int Inventory { get; set; }

        public OilTankerShip(int id, State state, int minPercantageCapacity, int maxCapacity) : base(id, state, minPercantageCapacity, maxCapacity)
        {
            Inventory = 0;
        }
    }
}
