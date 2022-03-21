using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborUWP.Models
{
    internal class Harbor
    {
        public List<DockingStation> DockingStations { get; set; }
        public Warehouse Warehouse { get; set; }

        public Harbor(Warehouse warehouse, List<DockingStation> dockingStation)
        {
            DockingStations = dockingStation;
            Warehouse = warehouse;
        }
    }
}
