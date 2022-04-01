using HarborUWP.Models.Enums;
using System;

namespace HarborUWP.Models.Ships.ShipFactory
{
    internal class BulkCarrierCreator : ShipCreator
    {
        public override Ship CreateProduct(int id)
        {
            Random random = new Random();
            int minPercantage = 80;
            int maxCapacity = random.Next(1000, 2000);
            int inventory = random.Next((int)(maxCapacity * (minPercantage / 100d)), maxCapacity);
            return new BulkCarrierShip(id, minPercantage, maxCapacity, inventory, GenerateBulkItemType(random));
        }

        private BulkItemType GenerateBulkItemType(Random random)
        {
            switch (random.Next(5))
            {
                case 0:
                default:
                    return BulkItemType.Coal;
                case 1:
                    return BulkItemType.Salt;
                case 2:
                    return BulkItemType.Sand;
                case 3:
                    return BulkItemType.Wheat;
                case 4:
                    return BulkItemType.Coal;
            }
        }
    }
}
