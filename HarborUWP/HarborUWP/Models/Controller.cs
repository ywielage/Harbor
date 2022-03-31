using HarborUWP.Models.Enums;
using HarborUWP.Models.Ships;
using HarborUWP.Models.Ships.ShipFactory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Collections.ObjectModel;
using Windows.UI.Core;

namespace HarborUWP.Models
{
    internal class Controller
    {
        public Harbor Harbor { get; set; }
        public ObservableCollection<Ship> Ships { get; set; }
        //aanpassen in de UI zodat je kan selecteren of het via threaded wordt gerunt om te bewijzen dat het threaded sneller is
        public bool runThreaded { get; set; }

        private System.Timers.Timer timer;

        private MainPage mainPage;
        public Controller()
        {
            runThreaded = false;
        }

        #region Initialization

        public void setMainPage(MainPage mainPage)
        {
            this.mainPage = mainPage;   
        }
        public void Initialize()
        {
            InitializeHarbor();
            InitializeShips();
            StartSimulation();
        }

        private void InitializeHarbor()
        {
            Warehouse warehouse = new Warehouse(0,0,0,0,0,0);
            Harbor = new Harbor(warehouse, InitializeDockingStation());
        }

        private List<DockingStation> InitializeDockingStation()
        {
            List<DockingStation> dockingStations = new List<DockingStation>();
            // 1/10 ratio voor DockingStation/Ship behouden
            for (int i = 0; i < 200; i++)
            {
                DockingStation dockingStation = new DockingStation(i);
                dockingStations.Add(dockingStation);
            }
            return dockingStations;
        }

        private void InitializeShips()
        {
            this.Ships = new ObservableCollection<Ship>();
            Random random = new Random();
            // 1/10 ratio voor DockingStation/Ship behouden
            //TODO: variabele voor amount of ships created, gebruiken in for loop 
            for (int i = 1; i <= 1000; i++)
            {
                int value = random.Next(3);
                this.Ships.Add(ShipCreator.CreateShip(Ship.GenerateRandomShipType(), i));
            }
        }
        #endregion

        #region Simulation
        private void StartSimulation()
        {
            this.timer = new System.Timers.Timer();
            //Interval in ms
            Debug.WriteLine("started Timer");
            this.timer.Interval = 400;
            //this.timer.Elapsed += tmr_Elapsed;
            this.timer.Elapsed += this.tmr_Elapsed;
            this.timer.Start();
        }

        public void tmr_Elapsed(object sender, EventArgs e)
        {   
            mainPage.updateUI(new List<String>{ this.UpdateShips().Last() });
            //foreach for manageInventory() die als het nodig is extra aan de Warehouse toevoegt. en dit als string returnt.
            foreach (String log in manageWarehouse())
            {
                Debug.WriteLine(log);
            }
        }

        public void StopSimulation()
        {
            this.timer.Elapsed -= this.tmr_Elapsed;
        }

        public void ContinueSimulation()
        {
            this.timer.Elapsed += this.tmr_Elapsed;
        }
        #endregion 
        #region Update
        public List<String> UpdateShips()
        {
            Debug.WriteLine("Updated");
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
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<String> returnList = new List<String>();
            List<DockingStation> dockingStationsList = new List<DockingStation>();
            dockingStationsList = this.Harbor.DockingStations;

            //loop door alle ships
            foreach (Ship ship in Ships)
            {
                //maak een update task die de update in de ships calt
                WaitCallback updateTask = (aShip) =>
                {
                    Ship selectedShip = aShip as Ship;
                    lock (dockingStationsList)
                    {
                        lock (returnList)
                        {
                            returnList.Add(UpdateShipTask(selectedShip, dockingStationsList));
                        }
                    }
                };

                //voeg de updateTask toe met een ship uit de loop
                ThreadPool.QueueUserWorkItem(updateTask, ship);
            }

            while (returnList.Count <= Ships.Count)
            {
                //zorg ervoor dat hij alleen stopt als hij klaar is met alle ships
                if (returnList.Count == Ships.Count)
                {
                    //stop de stopwatch om te meten of hij klaar is
                    stopwatch.Stop();
                    String timeToUpdate = stopwatch.Elapsed.TotalSeconds.ToString();
                    //voeg een string over de stopwatch terug aan de returnList
                    returnList.Add("Took " + timeToUpdate + " seconds to update " + Ships.Count + " ships, with threading");
                    return returnList;
                }
            }
            //Als er iets fout gaat:
            returnList.Add("Something went wrong");
            return returnList;
        }

        private List<String> UpdateShipsNonThreaded()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<String> returnList = new List<String>();
            List<DockingStation> dockingStationsList = new List<DockingStation>();
            dockingStationsList = this.Harbor.DockingStations;

            //loop door alle ships
            foreach (Ship ship in Ships)
            {
                returnList.Add(UpdateShipTask(ship, dockingStationsList));
            }
            stopwatch.Stop();
            //voeg een string over de stopwatch terug aan de returnList
            returnList.Add("Took " + stopwatch.Elapsed.TotalSeconds + " seconds to update " + Ships.Count + " ships, with threading");
            return returnList;
        }

        private String UpdateShipTask(Ship selectedShip, List<DockingStation> dockingStationsList)
        {
            State shipStateBefore = selectedShip.State;

            DockingStation availableDockingStation = null;

            //Haal een beschikbaar dockingStation op
            availableDockingStation = GetAvailableDockingStation(dockingStationsList);
            //Als er geen dockingstation beschikbaar is, verhoog de TimeUntilDone
            this.TryIncreaseShipTimeUntilDone(selectedShip, availableDockingStation);
            //Update de ship en sla het resultaat op
            String result = "\t" + selectedShip.Update();
            //Check of de shipState verandert is
            bool shipStateChanged = CheckShipStateChange(selectedShip, shipStateBefore);
            //Update de dockingstations als dat nodig is
            if (shipStateChanged)
            {
                this.UpdateDockingStations(selectedShip, availableDockingStation, shipStateBefore, dockingStationsList);
            }
            return result;

        }

        private DockingStation GetAvailableDockingStation(List<DockingStation> dockingStationsList)
        {
            DockingStation availableDockingStation = null;
            foreach (DockingStation dockingStation in dockingStationsList)
            {
                if (!dockingStation.IsOccupied())
                {
                    availableDockingStation = dockingStation;
                    break;
                }
            }
            return availableDockingStation;
        }

        private void TryIncreaseShipTimeUntilDone(Ship ship, DockingStation dockingStation)
        {
            if (dockingStation == null && ship.State.Equals(State.WaitingInPortWaters) && ship.TimeUntilDone.DurationInMins == 1)
            {
                ship.TimeUntilDone.DurationInMins++;
            }
        }

        private bool CheckShipStateChange(Ship ship, State stateBefore)
        {
            bool result = false;
            if (ship.State != stateBefore)
            {
                result = true;
            }
            return result;
        }

        private void UpdateDockingStations(Ship ship, DockingStation availableDockingStation, State shipStateBefore, List<DockingStation> dockingStationsList)
        {
            switch (ship.State)
            {
                case State.Docking:
                    availableDockingStation.DockShip(ship);
                    Debug.WriteLine("Docked ship" + ship.Id + " on station " + availableDockingStation.getNumber());
                    break;
                case State.Offloading:
                    ship.OffLoad(this.Harbor);
                    break;
                case State.Loading:
                    ship.Load(this.Harbor);
                    break;
                case State.InOpenWaters:
                    this.TryLeaveShip(ship, shipStateBefore, dockingStationsList);
                    break;
            }
        }

        private void TryLeaveShip(Ship ship, State shipStateBefore, List<DockingStation> dockingStationsList)
        {
            if (shipStateBefore == State.Leaving)
            {
                foreach (DockingStation dockingStation in dockingStationsList)
                {
                    if (dockingStation.GetShip() != null)
                    {
                        if (dockingStation.GetShip().Id == ship.Id)
                        {
                            Debug.WriteLine("Docking station " + dockingStation.getNumber() + " Is now empty");
                            dockingStation.LeaveShip();
                            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                            async () =>
                            {
                                this.Ships.Remove(ship);
                            });
                            this.AddNewShip();
                            break;
                        }
                    }
                }
            }
        }

        public List<String> manageWarehouse()
        {
            List<String> resultList = new List<String>();
                if (Harbor.Warehouse.TonsOfCoal.Amount == 0)
                {
                    Harbor.Warehouse.AddTonsOfCoal(100);
                    resultList.Add("Added Coal");
                }
                if (Harbor.Warehouse.TonsOfSand.Amount == 0)
                {
                    Harbor.Warehouse.AddTonsOfSand(100);
                    resultList.Add("Added Sand");
                }
                if (Harbor.Warehouse.TonsOfWheat.Amount == 0)
                {
                    Harbor.Warehouse.AddTonsOfWheat(100);
                    resultList.Add("Added Wheat");
                }
                if (Harbor.Warehouse.BarrelsOfOil.Amount == 0)
                {
                    Harbor.Warehouse.AddBarrelsOfOil(100);
                    resultList.Add("Added Oil");
                }
         /*   switch (Harbor.Warehouse)
            {
                case var expression when Harbor.Warehouse.TonsOfCoal.Amount == 0:
                    Harbor.Warehouse.AddTonsOfCoal(100);
                    resultList.Add("Added Coal");
                    Debug.WriteLine("works");
                    break;
                case var expression when Harbor.Warehouse.TonsOfSand.Amount == 0:
                    Harbor.Warehouse.AddTonsOfSand(100);
                    resultList.Add("Added Sand");
                    break;
                case var expression when Harbor.Warehouse.TonsOfWheat.Amount == 0:
                    Harbor.Warehouse.AddTonsOfWheat(100);
                    resultList.Add("Added Wheat");
                    break;
                case var expression when Harbor.Warehouse.BarrelsOfOil.Amount == 0:
                    Harbor.Warehouse.AddBarrelsOfOil(100);
                    resultList.Add("Added Oil");
                    break;
            }*/
            //TODO: if warehouse bijna leeg, this.Harbor.Warehouse.add, result.Add("")
            return resultList;
        }

        private void AddNewShip()
        {
            //add a new randow ship and add to ships list
        }

        #endregion
    }
}
