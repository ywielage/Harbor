using HarborUWP.Models.Enums;
using HarborUWP.Models.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborUWP.Models
{
    internal class Warehouse
    {
        public StorageObject BarrelsOfOil { get; }
        public StorageObject TonsOfSand { get; } 
        public StorageObject TonsOfCoal { get; }
        public StorageObject TonsOfWheat { get; }
        public List<Container> Containers { get; }

        public Warehouse()
        {
            BarrelsOfOil = new StorageObject(0);
            TonsOfSand = new StorageObject(0);
            TonsOfCoal = new StorageObject(0);
            TonsOfWheat = new StorageObject(0);
            Containers = new List<Container>();
        }

        public void AddBarrelsOfOil(int amount)
        {
            lock (BarrelsOfOil)
            {
                BarrelsOfOil.Amount += amount;
            }
        }

        public void RemoveBarrelsOfOil(int amount)
        {
            lock (BarrelsOfOil)
            {
                BarrelsOfOil.Amount -= amount;
            }
        }

        public void AddTonsOfSand(int amount)
        {
            lock (TonsOfSand)
            {
                TonsOfSand.Amount += amount;
            }
        }

        public void RemoveTonsOfSand(int amount)
        {
            lock (TonsOfSand)
            {
                TonsOfSand.Amount -= amount;
            }
        }

        public void AddTonsOfCoal(int amount)
        {
            lock (TonsOfCoal)
            {
                TonsOfCoal.Amount += amount;
            }
        }

        public void RemoveTonsOfCoal(int amount)
        {
            lock (TonsOfCoal)
            {
                TonsOfCoal.Amount -= amount;
            }
        }

        public void AddTonsOfWheat(int amount)
        {
            lock (TonsOfWheat)
            {
                TonsOfWheat.Amount += amount;
            }
        }

        public void RemoveTonsOfWheat(int amount)
        {
            lock (TonsOfWheat)
            {
                TonsOfWheat.Amount -= amount;
            }
        }

        public void AddContainers(List<Container> containers)
        {
            lock (Containers)
            {
                Containers.AddRange(containers);
            }
        }

        public void RemoveContainers(ContainerItemType itemType, int amount)
        {
            lock (Containers)
            {
                for (int i = 0; i < amount; i++)
                {
                    Containers.Remove(Containers.Find(c => c.ContainerItemType == itemType));
                }
            }
        }
    }
}
