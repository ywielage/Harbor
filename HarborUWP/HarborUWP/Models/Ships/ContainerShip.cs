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
        public override void OffLoad(Harbor harbor, int amount, ContainerItemType itemType)
        {
            for(int i = 0; i < amount; i++)
            {
                if(Containers.Count > 0)
                {
                    Containers.RemoveAt(Containers.Count - 1);
                    harbor.Warehouse.RemoveContainers()
                }
            }
        }

        public override void Load(Harbor harbor, int amount)
        {
            new Containe
        }
    }
}
