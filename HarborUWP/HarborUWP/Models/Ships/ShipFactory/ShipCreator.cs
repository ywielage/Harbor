using HarborUWP.Models.Enums;

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
