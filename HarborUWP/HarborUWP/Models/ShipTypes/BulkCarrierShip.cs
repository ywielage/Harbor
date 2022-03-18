using HarborApp.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborApp.Models.ShipTypes
{
    internal class BulkCarrierShip : Ship
    {
        public int Inventory { get; set; }
        public BulkItemType BulkItemType { get; set; }

        public BulkCarrierShip(int id, State state, int maxPercantageCapacity, int maxCapacity, int inventory, BulkItemType bulkItemType) : base(id, state, maxPercantageCapacity, maxCapacity)
        {
            Inventory = inventory;
            BulkItemType = bulkItemType;
        }
    }
}
