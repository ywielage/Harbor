using HarborUWP.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborUWP.Models.Ships
{
    internal class OilTankerShip : Ship
    {
        public int Inventory { get; set; }

        public OilTankerShip(int id, int minPercantageCapacity, int maxCapacity, int inventory) : base(id, minPercantageCapacity, maxCapacity)
        {
            Inventory = inventory;
        }
/*        public override void OffLoad(Harbor harbor, int amount)
        {
            Inventory -= amount;
            harbor.Warehouse.RemoveBarrelsOfOil(amount);
        }

        public override void Load(Harbor harbor, int amount)
        {
            Inventory -= amount;
            harbor.Warehouse.AddBarrelsOfOil(amount);
        }*/
    }
}
