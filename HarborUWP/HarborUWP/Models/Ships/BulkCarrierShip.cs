using HarborUWP.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborUWP.Models.Ships
{
    internal class BulkCarrierShip : Ship
    {
        public int Inventory { get; set; }
        public BulkItemType BulkItemType { get; set; }

        public BulkCarrierShip(int id, State state, int minPercantageCapacity, int maxCapacity, int inventory, BulkItemType bulkItemType) : base(id, state, minPercantageCapacity, maxCapacity)
        {
            Inventory = inventory;
            BulkItemType = bulkItemType;
        }
    }
}
