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

        public void DockShip(Ship ship)
        {
            this.Ship = ship;
        }

        public void LeaveShip()
        {
            this.Ship = null;
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
