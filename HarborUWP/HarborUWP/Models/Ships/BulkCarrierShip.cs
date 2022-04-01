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

        public BulkCarrierShip(int id, int minPercantageCapacity, int maxCapacity, int inventory, BulkItemType bulkItemType) : base(id, minPercantageCapacity, maxCapacity)
        {
            ShipType = ShipTypes.BulkCarrier;
            Inventory = inventory;
            BulkItemType = bulkItemType;
        }

        public override void OffLoad(Harbor harbor)
        {
            switch (BulkItemType)
            {
                case BulkItemType.Sand:
                    harbor.Warehouse.AddTonsOfSand(this.Inventory);
                    break;
                case BulkItemType.Salt:
                    harbor.Warehouse.AddTonsOfSalt(this.Inventory);
                    break;
                case BulkItemType.Coal:
                    harbor.Warehouse.AddTonsOfCoal(this.Inventory);
                    break;
                case BulkItemType.Wheat:
                    harbor.Warehouse.AddTonsOfWheat(this.Inventory);
                    break;
            }
            this.Inventory = 0;
        }

        public override void Load(Harbor harbor)
        {
            Inventory = new Random().Next((int)minPercentageCapacity, maxCapacity);
            switch (BulkItemType)
            {
                case BulkItemType.Sand:
                    harbor.Warehouse.RemoveTonsOfSand(Inventory);
                    break;
                case BulkItemType.Salt:
                    harbor.Warehouse.RemoveTonsOfSalt(Inventory);
                    break;
                case BulkItemType.Coal:
                    harbor.Warehouse.RemoveTonsOfCoal(Inventory);
                    break;
                case BulkItemType.Wheat:
                    harbor.Warehouse.RemoveTonsOfWheat(Inventory);
                    break;
            }
        }
    }
}
