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
        public string Id { get; set; }
        public int Size { get; set; }
        public ContainerItemType ContainerItemType { get; set; }

        public Container(string id, ContainerItemType containerItemType)
        {
            Id = id;
            ContainerItemType = containerItemType;
            Size = 4000;
        }

        public static List<Container> CreateContainers(int amount, int shipId)
        {
            List<Container> containerList = new List<Container>();

            for (int i = 0; i < amount; i++)
            {
                //aan de hand van het ship ID en de loop waarde zij er altijd uniek ID's
                string id = shipId + "-" + i;

                //random item type
                ContainerItemType type;
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
                containerList.Add(new Container(id, type));
            }
            return containerList;
        }
    }
}
