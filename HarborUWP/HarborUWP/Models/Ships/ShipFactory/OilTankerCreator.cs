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
            return new OilTankerShip(id, Enums.State.InOpenWaters, 80, random.Next(70000, 190000));
        }
    }
}
