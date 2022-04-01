using System;

namespace HarborUWP.Models.Ships.ShipFactory
{
    internal class OilTankerCreator : ShipCreator
    {
        public override Ship CreateProduct(int id)
        {
            Random random = new Random();
            int minPercantage = 80;
            int maxCapacity = random.Next(1000, 2000);
            int inventory = random.Next((int)(maxCapacity * (minPercantage / 100d)), maxCapacity);
            return new OilTankerShip(id, minPercantage, maxCapacity, inventory);
        }
    }
}
