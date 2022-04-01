using System.Collections.Generic;

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
