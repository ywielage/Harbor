using HarborUWP.Models.Enums;
using HarborUWP.Models.Exceptions;
using HarborUWP.Models.Ships;
using HarborUWP.Models.Ships.ShipFactory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace HarborUWP.Models
{
    internal class Controller
    {
        public Harbor Harbor { get; set; }
        public ObservableCollection<Ship> Ships { get; set; }
        //aanpassen in de UI zodat je kan selecteren of het via threaded wordt gerunt om te bewijzen dat het threaded sneller is
        public bool RunThreaded { get; set; }

        private int lastShipId;
        private int startAmountOfShip;
        private int startAmountOfDockingStation;

        private double amountOfUpdates = 0.0;
        private double avgTimeToUpdate = 0.0;

        private MainPage mainPage;
        public Controller()
        {
            RunThreaded = false;
        }

        #region Initialization

        public void setMainPage(MainPage mainPage)
        {
            this.mainPage = mainPage;
        }
        public string Initialize(int amountOfShips, int amountOfDockingStations)
        {
            startAmountOfShip = amountOfShips;
            startAmountOfDockingStation = amountOfDockingStations;
            var text = "";
            text+= InitializeHarbor() + "\n";
            text+= InitializeShips();
            StartSimulation();

            return text;
        }

        private string InitializeHarbor()
        {
            Warehouse warehouse = new Warehouse(200000, 20000, 20000, 20000, 20000, 2000);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var list = InitializeDockingStation();
            stopwatch.Stop();
            string timeToCreate = stopwatch.Elapsed.TotalSeconds.ToString();
            Harbor = new Harbor(warehouse, list);
            return "Took " + timeToCreate + " seconds to create " + startAmountOfDockingStation + " docking stations.";
        }

        private List<DockingStation> InitializeDockingStationNonThreaded()
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

        private List<DockingStation> InitializeDockingStationThreaded()
        {
            return Enumerable.Range(0,startAmountOfDockingStation).AsParallel().Select(i => new DockingStation(i)).ToList();
        }

        private List<DockingStation> InitializeDockingStation()
        {
            if (RunThreaded)
            {
                return InitializeDockingStationThreaded();
            }
            return InitializeDockingStationNonThreaded();
        }

        private string InitializeShips()
        {
            if (RunThreaded)
            {
                return InitializeShipsThreaded();
            }
            return InitializeShipsNonThreaded();
        }

        private string InitializeShipsThreaded()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.Ships = new ObservableCollection<Ship>(Enumerable.Range(0, startAmountOfShip).AsParallel().Select(i => ShipCreator.CreateShip(Ship.GenerateRandomShipType(), i)).ToList());
            stopwatch.Stop();
            string timeToCreate = stopwatch.Elapsed.TotalSeconds.ToString();

            lastShipId = startAmountOfShip;

            return "Took " + timeToCreate + " seconds to create " + startAmountOfShip + " Ships, with threading.";
        }

        private string InitializeShipsNonThreaded()
        {
            this.Ships = new ObservableCollection<Ship>();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            // 1/10 ratio voor DockingStation/Ship behouden
            for (int i = 1; i <= startAmountOfShip; i++)
            {
                this.Ships.Add(ShipCreator.CreateShip(Ship.GenerateRandomShipType(), i));
            }
            stopwatch.Stop();
            string timeToCreate = stopwatch.Elapsed.TotalSeconds.ToString();
            lastShipId = startAmountOfShip;

            return "Took " + timeToCreate + " seconds to create " + startAmountOfShip + " Ships, without threading.";
        }
        #endregion

        #region Simulation
        private void StartSimulation()
        {
            mainPage.update();
        }

        #endregion 

        #region Update
        public string UpdateShips()
        {
            if (RunThreaded)
            {
                return UpdateShipsThreaded();
            }
            return UpdateShipsNonThreaded();
        }

        private string UpdateShipsThreaded()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<DockingStation> dockingStationsList;
            dockingStationsList = this.Harbor.DockingStations;

            var threadCollection = new Task[Ships.Count];

            for (int i = 0; i < Ships.Count; i++)
            {
                Ship ship = Ships[i];
                threadCollection[i] = updateShipTask(ship, dockingStationsList);
            }

            Task.WaitAll(threadCollection);

            //stop de stopwatch om te meten of hij klaar is
            stopwatch.Stop();
            string timeToUpdate = stopwatch.Elapsed.TotalSeconds.ToString();

            //return een string over de stopwatch
            this.ChangeAVGUpdateTime(stopwatch.Elapsed.TotalSeconds);
            string avgString = this.avgTimeToUpdate.ToString();
            if (avgString.Length >= 9)
            {
                avgString = this.avgTimeToUpdate.ToString().Substring(0, 9);
            }
            return "Took " + timeToUpdate + " seconds to update " + Ships.Count + " ships, with threading. AVG: " + avgString;
        }

        private Task updateShipTask(Ship ship, List<DockingStation> dockingStationsList)
        {
            return Task.Run(() => UpdateShipTask(ship, dockingStationsList));
        }

        private string UpdateShipsNonThreaded()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<DockingStation> dockingStationsList;
            dockingStationsList = this.Harbor.DockingStations;

            //loop door alle ships
            for (int i = 0; i < Ships.Count; i++)
            {
                Ship ship = Ships[i];
                UpdateShipTask(ship, dockingStationsList);
            }
            stopwatch.Stop();
            //voeg een string over de stopwatch terug aan de returnList
            this.ChangeAVGUpdateTime(stopwatch.Elapsed.TotalSeconds);
            string avgString = this.avgTimeToUpdate.ToString();
            if (avgString.Length >= 9)
            {
                avgString = this.avgTimeToUpdate.ToString().Substring(0, 9);
            }
            return "Took " + stopwatch.Elapsed.TotalSeconds + " seconds to update " + Ships.Count + " ships, without threading. AVG: " + avgString;
        }

        private void UpdateShipTask(Ship selectedShip, List<DockingStation> dockingStationsList)
        {
            State shipStateBefore = selectedShip.State;

            //Update de ship en sla het resultaat op
            selectedShip.Update();
            //Check of de shipState verandert is
            bool shipStateChanged = CheckShipStateChange(selectedShip, shipStateBefore);
            //Update de dockingstations als dat nodig is
            if (shipStateChanged)
            {
                this.UpdateDockingStations(selectedShip, shipStateBefore, dockingStationsList);
            }
        }

        private DockingStation GetAvailableDockingStation(List<DockingStation> dockingStationsList)
        {
            DockingStation availableDockingStation = dockingStationsList.FirstOrDefault(d => !d.IsOccupied());
            return availableDockingStation;
        }

        private bool CheckShipStateChange(Ship ship, State stateBefore)
        {
            bool result = ship.State != stateBefore;
            return result;
        }

        private void UpdateDockingStations(Ship ship, State shipStateBefore, List<DockingStation> dockingStationsList)
        {
            switch (ship.State)
            {
                case State.Docking:
                    var availableDockingStation = GetAvailableDockingStation(dockingStationsList);

                    var docked = false;
                    if (availableDockingStation != null)
                    {
                        docked = availableDockingStation.DockShipIfAvailable(ship);
                    }
                    //if the ship could not dock,  set its state back to waiting
                    if (!docked)
                    {
                        ship.State = State.WaitingInPortWaters;
                        ship.TimeUntilDone.DurationInMins = 1;
                    }
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
                dockingStationsList.FirstOrDefault(d => d.GetShip()?.Id == ship.Id)?.LeaveShip();

                _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    this.Ships.Remove(ship);
                    this.AddNewShip();
                });
            }
        }

        public void ChangeAVGUpdateTime(double timeToUpdate)
        {
            this.amountOfUpdates += 1.0;
            double totalTimeToUpdateBefore = this.avgTimeToUpdate * (amountOfUpdates - 1);
            double totalTimeToUpdate = totalTimeToUpdateBefore + timeToUpdate;
            double avgTimeToUpdate0 = totalTimeToUpdate / this.amountOfUpdates;
            this.avgTimeToUpdate = avgTimeToUpdate0;
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
