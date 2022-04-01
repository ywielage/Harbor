using System;

namespace HarborUWP.Models.Ships.ShipFactory
{
    internal class ContainerCreator : ShipCreator
    {
        public override Ship CreateProduct(int id)
        {
            Random random = new Random();
            int minPercantage = 80;
            int maxCapacity = random.Next(100, 200);
            int amountOfContainers = random.Next((int)(maxCapacity * (minPercantage / 100d)), maxCapacity);
            return new ContainerShip(id, minPercantage, maxCapacity, amountOfContainers);
        }
    }
}
