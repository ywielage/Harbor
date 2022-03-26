using HarborUWP.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborUWP.Models.Ships
{
    internal class OilTankerShip : Ship
    {
        public int Inventory { get; set; }

        public OilTankerShip(int id, int minPercantageCapacity, int maxCapacity) : base(id, minPercantageCapacity, maxCapacity)
        {
            Inventory = 100000;
        }
    }
}
