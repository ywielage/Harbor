using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborUWP.Models
{
    internal class Warehouse
    {
        public int BarrelsOfOil { get; set; }
        public int TonsOfSand { get; set; } 
        public int TonsOfCoal { get; set; }
        public int TonsOfWheat { get; set; }
        public List<Container> containers { get; set; }

        public Warehouse()
        {
            BarrelsOfOil = 0;
            TonsOfSand = 0;
            TonsOfCoal = 0;
            TonsOfWheat = 0;
            containers = new List<Container>();
        }
    }
}
