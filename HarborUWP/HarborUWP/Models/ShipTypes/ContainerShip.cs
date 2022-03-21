using HarborUWP.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborUWP.Models.ShipTypes
{
    internal class ContainerShip : Ship
    {
        public List<Container> containers { get; set; }

        public ContainerShip(int id, State state, int minPercantageCapacity, int maxCapacity) : base(id, state, minPercantageCapacity, maxCapacity)
        {
            containers = new List<Container>();
        }
    }
}
