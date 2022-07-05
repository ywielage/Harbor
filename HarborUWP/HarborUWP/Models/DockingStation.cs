using HarborUWP.Models.Ships;

namespace HarborUWP.Models
{
    internal class DockingStation
    {
        public int Number { get; set; }
        public Ship Ship { get; set; }

        public DockingStation(int number)
        {
            Number = number;
            Ship = null;
        }

        public DockingStation()
        {

        }

        public Ship GetShip()
        {
            return Ship;
        }

        public bool IsOccupied()
        {
            return Ship != null;
        }

        readonly object _lock = new object();

        public bool DockShipIfAvailable(Ship ship)
        {
            lock (_lock)
            {
                if (!IsOccupied())
                {
                    this.Ship = ship;
                    return true;
                }
            }

            return false;
        }

        public void LeaveShip()
        {
            lock (_lock)
            {
                if (this.Ship != null)
                {
                    this.Ship = null;
                }
            }
        }

        public int getNumber()
        {
            return this.Number;
        }

        public DockingStation Clone()
        {
            DockingStation dockingStation = new DockingStation();
            dockingStation.Number = this.Number;
            dockingStation.Ship = this.Ship;
            return dockingStation;
        }
    }
}
