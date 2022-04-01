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

        private int lastShipId;
        private int startAmountOfShip = 10000;
        private int startAmountOfDockingStation = 1000;
        private double amountOfUpdates = 0.0;
        private double avgTimeToUpdate = 0.0;

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
            Debug.WriteLine("started initialization thread");
            InitializeHarbor();
            InitializeShips();
            StartSimulation();
            Debug.WriteLine("Done initializing");
        }

        private void InitializeHarbor()
        {
            Warehouse warehouse = new Warehouse(190000, 190000, 190000, 190000, 190000, 0);
            Harbor = new Harbor(warehouse, InitializeDockingStation());
        }

        private List<DockingStation> InitializeDockingStation()
        {
            List<DockingStation> dockingStations = new List<DockingStation>();
            // 1/10 ratio voor DockingStation/Ship behouden
            for (int i = 0; i < startAmountOfDockingStation; i++)
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
            for (int i = 1; i <= startAmountOfShip; i++)
            {
                int value = random.Next(3);
                this.Ships.Add(ShipCreator.CreateShip(Ship.GenerateRandomShipType(), i));
            }
            lastShipId = startAmountOfShip;
        }
        #endregion

        #region Simulation
        private void StartSimulation()
        {
            this.timer = new System.Timers.Timer();
            //Interval in ms
            Debug.WriteLine("started Timer");
            this.timer.Interval = 3000;
            //this.timer.Elapsed += tmr_Elapsed;
            this.timer.Elapsed += this.tmr_Elapsed;
            this.timer.Start();
        }

        public void tmr_Elapsed(object sender, EventArgs e)
        {   
            //aanpassen naar normale string
            List<String> results = new List<String>();
            results.Add(UpdateShips());
            mainPage.updateUI(results);
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
        public String UpdateShips()
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

        private String UpdateShipsThreaded()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<DockingStation> dockingStationsList = new List<DockingStation>();
            dockingStationsList = this.Harbor.DockingStations;

            CountdownEvent countdown = new CountdownEvent(1);
            //loop door alle ships
            for (int i = 0; i < Ships.Count; i++)
            {
                countdown.AddCount();
                Ship ship = Ships[i];
                //maak een update task die de update in de ships calt
                WaitCallback updateTask = (aShip) =>
                {
                    Ship selectedShip = aShip as Ship;
                    lock (dockingStationsList)
                    {
                        UpdateShipTask(selectedShip, dockingStationsList);
                    }
                    countdown.Signal();
                };
                //voeg de updateTask toe met een ship uit de loop
                ThreadPool.QueueUserWorkItem(updateTask, ship);
            }
            countdown.Signal();
            countdown.Wait();
            countdown.Reset();
            //stop de stopwatch om te meten of hij klaar is
            stopwatch.Stop();
            String timeToUpdate = stopwatch.Elapsed.TotalSeconds.ToString();
            //return een string over de stopwatch
            this.changeAVGUpdateTime(stopwatch.Elapsed.TotalSeconds);
            return "Took " + timeToUpdate + " seconds to update " + Ships.Count + " ships, with threading. AVG: " + this.avgTimeToUpdate;
        }

        private String UpdateShipsNonThreaded()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<DockingStation> dockingStationsList = new List<DockingStation>();
            dockingStationsList = this.Harbor.DockingStations;

            //loop door alle ships
            for (int i = 0; i < Ships.Count; i++)
            {
                Ship ship = Ships[i];
                UpdateShipTask(ship, dockingStationsList);
            }
            stopwatch.Stop();
            //voeg een string over de stopwatch terug aan de returnList
            this.changeAVGUpdateTime(stopwatch.Elapsed.TotalSeconds);
            return "Took " + stopwatch.Elapsed.TotalSeconds + " seconds to update " + Ships.Count + " ships, without threading. AVG: " + this.avgTimeToUpdate;
        }

        private void UpdateShipTask(Ship selectedShip, List<DockingStation> dockingStationsList)
        {
            State shipStateBefore = selectedShip.State;

            DockingStation availableDockingStation = null;

            //Haal een beschikbaar dockingStation op
            availableDockingStation = GetAvailableDockingStation(dockingStationsList);
            //Als er geen dockingstation beschikbaar is, verhoog de TimeUntilDone
            this.TryIncreaseShipTimeUntilDone(selectedShip, availableDockingStation);
            //Update de ship en sla het resultaat op
            selectedShip.Update();
            //Check of de shipState verandert is
            bool shipStateChanged = CheckShipStateChange(selectedShip, shipStateBefore);
            //Update de dockingstations als dat nodig is
            if (shipStateChanged)
            {
                this.UpdateDockingStations(selectedShip, availableDockingStation, shipStateBefore, dockingStationsList);
            }

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
                            _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,() =>
                            {
                                this.Ships.Remove(ship);
                                this.AddNewShip();
                            });
                            break;
                        }
                    }
                }
            }
        }

        public void changeAVGUpdateTime(double timeToUpdate)
        {
            this.amountOfUpdates += 1.0;
            double totalTimeToUpdateBefore = this.avgTimeToUpdate * (amountOfUpdates - 1);
            double totalTimeToUpdate = totalTimeToUpdateBefore + timeToUpdate;
            double avgTimeToUpdate0 = totalTimeToUpdate / this.amountOfUpdates;
            this.avgTimeToUpdate = avgTimeToUpdate0;
        }

        public List<String> manageWarehouse()
        {
            List<String> resultList = new List<String>();
                if (Harbor.Warehouse.TonsOfCoal.Amount < 20000)
                {
                    Harbor.Warehouse.AddTonsOfCoal(this.Ships.Count * 20000);
                    resultList.Add("Added Coal");
                }
                if (Harbor.Warehouse.TonsOfSand.Amount < 20000)
                {
                    Harbor.Warehouse.AddTonsOfSand(1000000);
                    resultList.Add("Added Sand");
                }
                if (Harbor.Warehouse.TonsOfWheat.Amount < 20000)
                {
                    Harbor.Warehouse.AddTonsOfWheat(this.Ships.Count * 20000);
                    resultList.Add("Added Wheat");
                }
                if (Harbor.Warehouse.BarrelsOfOil.Amount < 190000)
                {
                    Harbor.Warehouse.AddBarrelsOfOil(this.Ships.Count * 190000);
                    resultList.Add("Added Oil");
                }
                if (Harbor.Warehouse.TonsOfSalt.Amount < 20000)
                {
                    Harbor.Warehouse.AddTonsOfSalt(this.Ships.Count * 20000);
                    resultList.Add("Added Salt");
                }
            return resultList;
        }

        private void AddNewShip()
        {
            //add a new randow ship and add to ships list
            lastShipId++;
            this.Ships.Add(ShipCreator.CreateShip(Ship.GenerateRandomShipType(), lastShipId));
        }

        #endregion
    }
}
