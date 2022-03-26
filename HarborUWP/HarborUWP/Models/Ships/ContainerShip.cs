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
        public int totalWeight { get; set; }

        public ContainerShip(int id, int minPercantageCapacity, int maxCapacity) : base(id, minPercantageCapacity, maxCapacity)
        {
            Containers = new List<Container>();
            CreateContainers(5, 0, 4000);
        }

        public void CreateContainers(int size, int ShipId, int weight)
        {
            for (int i = 0; i < size; i++)
            {
                //aan de hand van het ship ID en de loop waarde zij er altijd uniek ID's
                int id = Convert.ToInt32("" + ShipId + i);

                //random item type
                Enums.ContainerItemType type;
                switch (new Random().Next(5))
                {
                    case 0:
                    default:
                        type = ContainerItemType.Equipment;
                        break;
                    case 1:
                        type = ContainerItemType.Furniture;
                        break;
                    case 2:
                        type = ContainerItemType.Provisions;
                        break;
                    case 3:
                        type = ContainerItemType.Vehicles;
                        break;
                    case 4:
                        type = ContainerItemType.Equipment;
                        break;
                }
                Containers.Add(new Container(id, type, weight));
                totalWeight += weight;
            }
        }
    }
}
