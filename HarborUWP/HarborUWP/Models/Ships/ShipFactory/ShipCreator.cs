using HarborUWP.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborUWP.Models.Ships.ShipFactory
{
    internal abstract class ShipCreator
    {
        public abstract Ship CreateProduct(int id);

        public static Ship CreateShip(ShipTypes shipType, int id)
        {
            switch (shipType)
            {
                case ShipTypes.BulkCarrier:
                default:
                    return new BulkCarrierCreator().CreateProduct(id);
                case ShipTypes.Container:
                    return new ContainerCreator().CreateProduct(id);
                case ShipTypes.OilTanker:
                    return new OilTankerCreator().CreateProduct(id);
            }
        }
    }
}
