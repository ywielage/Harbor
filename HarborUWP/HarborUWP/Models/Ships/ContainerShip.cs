using HarborUWP.Models.Enums;
using System;
using System.Collections.Generic;

namespace HarborUWP.Models.Ships
{
    internal class ContainerShip : Ship
    {
        public List<Container> Containers { get; set; }
        public int TotalWeight { get; set; }

        public ContainerShip(int id, int minPercantageCapacity, int maxCapacity, int amountOfContainers) : base(id, minPercantageCapacity, maxCapacity)
        {
            ShipType = ShipTypes.Container;
            Containers = new List<Container>();
            Containers.AddRange(Container.CreateContainers(amountOfContainers, id));
        }
        // Offload alle containers op schip
        public override void OffLoad(Harbor harbor)
        {
            harbor.Warehouse.AddContainers(Containers);
        }

        // Random getal tussen minpercentage en maxCapacity
        // Hoogste itemtype pakken
        public override void Load(Harbor harbor)
        {
            harbor.Warehouse.RemoveContainers(new Random().Next((int)GetMinPercentageCapacity(), GetMaxCapacity()));
        }
    }
}
