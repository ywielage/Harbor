using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborUWP.Models.Ships.ShipFactory
{
    internal class OilTankerCreator : ShipCreator
    {
        public override Ship CreateProduct(int id)
        {
            Random random = new Random();
            int minPercantage = 80;
            int maxCapacity = random.Next(70000, 190000);
            int inventory = random.Next((int)(maxCapacity * (minPercantage / 100)), maxCapacity);
            return new OilTankerShip(id, 80, maxCapacity, inventory);
        }
    }
}
