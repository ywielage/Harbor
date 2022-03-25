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
            int size = random.Next(17000, 20000);
            //add createContatainerShip()
            return new ContainerShip(id, 80, size);
        }
    }
}
