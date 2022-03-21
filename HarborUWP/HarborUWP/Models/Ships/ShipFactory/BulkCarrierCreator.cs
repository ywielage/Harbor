using HarborUWP.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborUWP.Models.Ships.ShipFactory
{
    internal class BulkCarrierCreator : ShipCreator
    {
        public override Ship CreateProduct(int id)
        {
            Random random = new Random();
            return new BulkCarrierShip(id, State.InOpenWaters, 80, random.Next(18000, 20000), 0, GenerateBulkItemType(random));
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
