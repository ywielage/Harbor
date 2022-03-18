using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborApp.Models
{
    internal class Harbor
    {
        public List<DockingStation> DockingStations { get; set; }
        public Warehouse Warehouse { get; set; }

        public Harbor(Warehouse warehouse)
        {
            DockingStations = new();
            Warehouse = warehouse;
        }
    }
}
