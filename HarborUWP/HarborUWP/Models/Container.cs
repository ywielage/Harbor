using HarborUWP.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborUWP.Models
{
    internal class Container
    {
        public int Id { get; set; }
        public ContainerItemType ContainerItemType { get; set; }

        public Container(int id, ContainerItemType containerItemType)
        {
            Id = id;
            ContainerItemType = containerItemType;
        }

        public List<Container> CreateContainers(int size, int ShipId)
        {
            List<Container> containers = new List<Container>();
            Random random = new Random();
            for (int i = 0; i < size; i++)
            {
                //aan de hand van het ship ID en de loop waarde zij er altijd uniek ID's
                int id = Convert.ToInt32("" + ShipId + i);

                //random item type
                Enums.ContainerItemType type;
                switch (random.Next(5))
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

                containers.Add(new Container(id, type));
            }
            return containers;
        }
    }
}
