using HarborUWP.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborUWP.Models.Ships
{
    internal class ContainerShip : Ship
    {
        public List<Container> Containers { get; set; }

        public ContainerShip(int id, int minPercantageCapacity, int maxCapacity) : base(id, minPercantageCapacity, maxCapacity)
        {
            Containers = new List<Container>();
        }
    }
}
