using HarborUWP.Models.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HarborUWP.Models
{
    internal class Warehouse
    {
        public StorageObject BarrelsOfOil { get; }
        public StorageObject TonsOfSand { get; }
        public StorageObject TonsOfCoal { get; }
        public StorageObject TonsOfWheat { get; }
        public StorageObject TonsOfSalt { get; }
        public List<Container> Containers { get; }
        public ObservableCollection<WarehouseItemViewModel> WarehouseView { get; set; }
        public StorageObject AmountOfEquipmentContainers { get; }
        public StorageObject AmountOfFurnitureContainers { get; }
        public StorageObject AmountOfProvisionsContainers { get; }
        public StorageObject AmountOfVehicleContainers { get; }

        public Warehouse(int amountOfBarrelsOfOil, int amountOfTonsOfSand, int amountOfTonsOfCoal, int amountOfTonsOfWheat, int amountOfTonsOfSalt, int amountOfContainers)
        {
            BarrelsOfOil = new StorageObject(amountOfBarrelsOfOil);
            TonsOfSand = new StorageObject(amountOfTonsOfSand);
            TonsOfCoal = new StorageObject(amountOfTonsOfCoal);
            TonsOfWheat = new StorageObject(amountOfTonsOfWheat);
            TonsOfSalt = new StorageObject(amountOfTonsOfSalt);
            Containers = new List<Container>();
            AmountOfEquipmentContainers = new StorageObject(0);
            AmountOfFurnitureContainers = new StorageObject(0);
            AmountOfProvisionsContainers = new StorageObject(0);
            AmountOfVehicleContainers = new StorageObject(0);
            Containers.AddRange(Container.CreateContainers(amountOfContainers, 0));
            IncreaseContainerCounters(Containers);
            WarehouseView = CreateViewCollection();
        }

        private ObservableCollection<WarehouseItemViewModel> CreateViewCollection()
        {
            return new ObservableCollection<WarehouseItemViewModel>()
            {
                new WarehouseItemViewModel()
                {
                    Name = "Barrels of Oil",
                    StorageObject = BarrelsOfOil
                },
                new WarehouseItemViewModel()
                {
                    Name = "Tons of Sand",
                    StorageObject = TonsOfSand
                },
                new WarehouseItemViewModel()
                {
                    Name = "Tons of Coal",
                    StorageObject = TonsOfCoal
                },
                new WarehouseItemViewModel()
                {
                    Name = "Tons of Wheat",
                    StorageObject = TonsOfWheat
                },
                new WarehouseItemViewModel()
                {
                    Name = "Tons of Salt",
                    StorageObject = TonsOfSalt
                },
                new WarehouseItemViewModel()
                {
                    Name = "Equipment containers",
                    StorageObject = AmountOfEquipmentContainers
                },
                new WarehouseItemViewModel()
                {
                    Name = "Furniture containers",
                    StorageObject = AmountOfFurnitureContainers
                },
                new WarehouseItemViewModel()
                {
                    Name = "Provisions containers",
                    StorageObject = AmountOfProvisionsContainers
                },
                new WarehouseItemViewModel()
                {
                    Name = "Vehicle containers",
                    StorageObject = AmountOfVehicleContainers
                },

            };
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

        public void AddTonsOfSalt(int amount)
        {
            lock (TonsOfSalt)
            {
                TonsOfSalt.Amount += amount;
            }
        }

        public void RemoveTonsOfSalt(int amount)
        {
            lock (TonsOfSalt)
            {
                TonsOfSalt.Amount -= amount;
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

                IncreaseContainerCounters(containers);
            }
        }

        public void RemoveContainers(int amount)
        {
            lock (Containers)
            {
                var containers = (from Container in Containers select Container).Take(amount).ToArray();
                foreach (Container container in containers)
                {
                    switch (container.ContainerItemType)
                    {
                        default:
                            AmountOfVehicleContainers.Amount -= 1;
                            break;
                        case ContainerItemType.Equipment:
                            AmountOfEquipmentContainers.Amount -= 1;
                            break;
                        case ContainerItemType.Provisions:
                            AmountOfProvisionsContainers.Amount -= 1;
                            break;
                        case ContainerItemType.Furniture:
                            AmountOfFurnitureContainers.Amount -= 1;
                            break;
                    }
                }
                Containers.RemoveAll(c => containers.Contains(c));
            }
        }

        private void IncreaseContainerCounters(List<Container> containers)
        {
            foreach (Container container in containers)
            {
                switch (container.ContainerItemType)
                {
                    case ContainerItemType.Vehicles:
                    default:
                        AmountOfVehicleContainers.Amount += 1;
                        break;
                    case ContainerItemType.Equipment:
                        AmountOfEquipmentContainers.Amount += 1;
                        break;
                    case ContainerItemType.Provisions:
                        AmountOfProvisionsContainers.Amount += 1;
                        break;
                    case ContainerItemType.Furniture:
                        AmountOfFurnitureContainers.Amount += 1;
                        break;
                }
            }
        }
    }
}
