using HarborApp.Models.ShipTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace HarborApp.Models
{
    internal class Controller
    {
        public Harbor Harbor { get; set; }
        public List<Ship> Ships { get; set; }
        //aanpassen in de UI zodat je kan selecteren of het via threaded wordt gerunt om te bewijzen dat het threaded sneller is
        private bool runThreaded;

        private Timer timer;

        public Controller()
        {
            Debug.WriteLine("started Controller");
            initialize();
            runThreaded = false;
        }

        private void initialize()
        {
            initializeHarbor();
            initializeShips();
            startSimulation();
        }
        private void initializeHarbor()
        {
            Warehouse warehouse = new Warehouse();
            Harbor = new Harbor(warehouse, initializeDockingStation());
        }
        private List<DockingStation> initializeDockingStation()
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
        private void initializeShips()
        {
            this.Ships = new List<Ship>();
            // 1/10 ratio voor DockingStation/Ship behouden
            for (int i = 0; i < 100; i++)
            {
                this.Ships.Add(createRandomShip(i));
            }
        }
        //update ship constructor met de automatische 80% voor maxPercentageCapacity
        private Ship createRandomShip(int id)
        {
            Random random = new Random();
            int value = random.Next(3);
            Ship ship;
            switch (value)
            {
                case 0:
                    ship = createBulkCarrierShip(id);
                    break;
                case 1:
                    ship = createOilTankerShip(id);
                    break;
                case 2:
                    ship = createContainerShip(id);
                    break;
                default:
                    ship = createBulkCarrierShip(id);
                    break;
            }
            return ship;
        }

        private Ship createBulkCarrierShip(int id)
        {
            Ship ship;
            Random random = new Random();

            //random type selecteren
            Enums.BulkItemType type;
            switch (random.Next(4))
            {
                case 0:
                    type = Enums.BulkItemType.Coal;
                    break;
                case 1:
                    type = Enums.BulkItemType.Salt;
                    break;
                case 2:
                    type = Enums.BulkItemType.Sand;
                    break;
                case 3:
                    type = Enums.BulkItemType.Wheat;
                    break;
                default:
                    type = Enums.BulkItemType.Coal;
                    break;
            }
            
            ship = new BulkCarrierShip(id, Enums.State.InOpenWaters, 80, random.Next(18000, 20000), 0, type);
            return ship;
        }

        private Ship createOilTankerShip(int id)
        {
            Random random = new Random();
            return new OilTankerShip(id, Enums.State.InOpenWaters, 80, random.Next(70000, 190000));
        }

        private Ship createContainerShip(int id)
        {
            Random random = new Random();
            int size = random.Next(17000, 20000);
            //add createContatainerShip()
            return new ContainerShip(id, Enums.State.InOpenWaters, 80, size);
        }

        private List<Container> createContainers(int size, int ShipId)
        {
            List<Container> containers = new List<Container>();
            Random random = new Random();   
            for (int i = 0; i < size; i++)
            {
                //aan de hand van het ship ID en de loop waarde zij er altijd uniek ID's
                int id = Convert.ToInt32("" + ShipId + i);

                //random item type
                Enums.ContainerItemType type;
                switch (random.Next(4))
                {
                    case 0:
                        type = Enums.ContainerItemType.Equipment;
                        break;
                    case 1:
                        type = Enums.ContainerItemType.Furniture;
                        break;
                    case 2:
                        type = Enums.ContainerItemType.Provisions;
                        break;
                    case 3:
                        type = Enums.ContainerItemType.Vehicles;
                        break;
                    default:
                        type = Enums.ContainerItemType.Equipment;
                        break;
                }

                containers.Add(new Container(id, type));
            }
            return containers;
        }

        private void startSimulation()
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

        public void stopSimulation()
        {
            this.timer.Stop();
        }

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
    }
}
