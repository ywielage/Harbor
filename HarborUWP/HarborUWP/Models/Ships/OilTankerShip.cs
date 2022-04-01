using HarborUWP.Models.Enums;
using System;

namespace HarborUWP.Models.Ships
{
    internal class OilTankerShip : Ship
    {
        public int Inventory { get; set; }

        public OilTankerShip(int id, int minPercantageCapacity, int maxCapacity, int inventory) : base(id, minPercantageCapacity, maxCapacity)
        {
            ShipType = ShipTypes.OilTanker;
            Inventory = inventory;
        }
        public override void OffLoad(Harbor harbor)
        {
            harbor.Warehouse.AddBarrelsOfOil(Inventory);
            Inventory = 0;
        }

        public override void Load(Harbor harbor)
        {
            Inventory = new Random().Next((int)GetMinPercentageCapacity(), GetMaxCapacity());
            harbor.Warehouse.RemoveBarrelsOfOil(Inventory);
        }
    }
}
