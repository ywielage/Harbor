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
        public int TotalWeight { get; set; }

        public ContainerShip(int id, int minPercantageCapacity, int maxCapacity, int amountOfContainers) : base(id, minPercantageCapacity, maxCapacity)
        {
            Containers = new List<Container>();
            Container.CreateContainers(amountOfContainers, id);
        }
        // Offload alle containers op schip
        public override void OffLoad(Harbor harbor)
        {
            this.Containers.ForEach(container =>
            harbor.Warehouse.Containers.Add(container));
            this.Containers.Clear();
        }

        // Random getal tussen minpercentage en maxCapacity
        // Hoogste itemtype pakken
        public override void Load(Harbor harbor)
        {
            harbor.Warehouse.RemoveContainers(new Random().Next((int)Math.Round(maxCapacity * 0.8), maxCapacity));
        }
    }
}
