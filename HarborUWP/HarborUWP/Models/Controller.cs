using HarborUWP.Models.Enums;
using HarborUWP.Models.Ships;
using HarborUWP.Models.Ships.ShipFactory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace HarborUWP.Models
{
    internal class Controller
    {
        public Harbor Harbor { get; set; }
        public List<Ship> Ships { get; set; }
        //aanpassen in de UI zodat je kan selecteren of het via threaded wordt gerunt om te bewijzen dat het threaded sneller is
        public bool runThreaded { get; set; }

        private Timer timer;

        public Controller()
        {
            runThreaded = false;
        }

        #region Initialization
        public void Initialize()
        {
            InitializeHarbor();
            InitializeShips();
            StartSimulation();
        }

        private void InitializeHarbor()
        {
            Warehouse warehouse = new Warehouse();
            Harbor = new Harbor(warehouse, InitializeDockingStation());
        }

        private List<DockingStation> InitializeDockingStation()
        {
            List<DockingStation> dockingStations = new List<DockingStation>();
            // 1/10 ratio voor DockingStation/Ship behouden
            for (int i = 0; i < 10; i++)
            {
                DockingStation dockingStation = new DockingStation(i);
                dockingStations.Add(dockingStation);
            }
            return dockingStations;
        }

        private void InitializeShips()
        {
            this.Ships = new List<Ship>();
            Random random = new Random();
            // 1/10 ratio voor DockingStation/Ship behouden
            for (int i = 0; i < 100; i++)
            {
                int value = random.Next(3);
                this.Ships.Add(ShipCreator.CreateShip(Ship.GenerateRandomShipType(), i));
            }
        }
        #endregion

        #region Simulation
        private void StartSimulation()
        {
            this.timer = new Timer();
            //Interval in ms
            this.timer.Interval = 2000;
            this.timer.Elapsed += tmr_Elapsed;
            this.timer.Start();
        }

        public void tmr_Elapsed(object sender, EventArgs e)
        {
            foreach (String log in UpdateShips())
            {
                Debug.WriteLine(log);
            }
        }

        public void StopSimulation()
        {
            this.timer.Stop();
        }
        #endregion 

        #region Update
        public List<String> UpdateShips()
        {

            if (runThreaded)
            {
                return UpdateShipsThreaded();
            }
            else
            {
                return UpdateShipsNonThreaded();
            }
        }

        private List<String> UpdateShipsThreaded()
        {
            List<String> returnList = new List<String>();
            foreach (Ship ship in Ships)
            {
                //call de update in de ships die als het goed is een string van wat er is gebeurt returnt
                //returnList.Add(ship.Update());
                returnList.Add("Threaded ship updated" + ship.Id + ship.GetType());
            }
            return returnList;
        }

        private List<String> UpdateShipsNonThreaded()
        {
            List<String> returnList = new List<String>();
            foreach (Ship ship in Ships)
            {
                //call de update in de ships die als het goed is een string van wat er is gebeurt returnt
                //returnList.Add(ship.Update());
                returnList.Add("Non Threaded ship updated" + ship.Id + ship.GetType());
            }
            return returnList;
        }
        #endregion
    }
}
