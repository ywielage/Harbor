using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborUWP.Models.Ships.ShipFactory
{
    internal class ContainerCreator : ShipCreator
    {
        public override Ship CreateProduct(int id)
        {
            Random random = new Random();
            int minPercantage = 80;
            int maxCapacity = random.Next(3000, 4000);
            int amountOfContainers = random.Next((int)(maxCapacity * (minPercantage / 100)), maxCapacity);
            return new ContainerShip(id, 80, maxCapacity, amountOfContainers);
        }
    }
}
