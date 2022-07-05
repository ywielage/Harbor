using HarborUWP.Models.Enums;
using HarborUWP.Models.Exceptions;
using HarborUWP.Models.Ships;
using HarborUWP.Models.Ships.ShipFactory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        private System.Timers.Timer timer;

        private int lastShipId;
        private int startAmountOfShip;
        private int startAmountOfDockingStation;
        private int timerTimeInMs = 1500;

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
        public void Initialize(int amountOfShips, int amountOfDockingStations)
        {
            startAmountOfShip = amountOfShips;
            startAmountOfDockingStation = amountOfDockingStations;

            InitializeHarbor();
            InitializeShips();
            StartSimulation();
        }

        private void InitializeHarbor()
        {
            Warehouse warehouse = new Warehouse(200000, 20000, 20000, 20000, 20000, 2000);
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
            if (RunThreaded)
            {
                InitializeShipsThreaded();
            }
            else
            {
                InitializeShipsNonThreaded();
            }
        }

        private void InitializeShipsThreaded()
        {
            var ships = new ConcurrentBag<Ship>();

            Parallel.For(0, startAmountOfShip, i =>
            {
                ships.Add(ShipCreator.CreateShip(Ship.GenerateRandomShipType(), i));
            });

            this.Ships = new ObservableCollection<Ship>(ships);

            lastShipId = startAmountOfShip;
        }

        private void InitializeShipsNonThreaded()
        {
            this.Ships = new ObservableCollection<Ship>();
            Random random = new Random();
            // 1/10 ratio voor DockingStation/Ship behouden
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
            this.timer.Interval = this.timerTimeInMs;
            //this.timer.Elapsed += tmr_Elapsed;
            this.timer.Elapsed += this.tmr_Elapsed;
            this.timer.Start();
        }

        public void tmr_Elapsed(object sender, EventArgs e)
        {
            //aanpassen naar normale string
            String result = (UpdateShips());
            mainPage.updateUI(result);
            //foreach for manageInventory() die als het nodig is extra aan de Warehouse toevoegt. en dit als string returnt.
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
            if (RunThreaded)
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

            if (stopwatch.Elapsed.TotalSeconds >= (Convert.ToDouble(this.timerTimeInMs) / 1000))
            {
                throw new UpdateTookLongerThanTimerException(stopwatch.Elapsed.TotalSeconds, this.timerTimeInMs);
            }

            //return een string over de stopwatch
            this.ChangeAVGUpdateTime(stopwatch.Elapsed.TotalSeconds);
            String avgString = this.avgTimeToUpdate.ToString();
            if (avgString.Length >= 9)
            {
                avgString = this.avgTimeToUpdate.ToString().Substring(0, 9);
            }
            return "Took " + timeToUpdate + " seconds to update " + Ships.Count + " ships, with threading. AVG: " + avgString;
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
            this.ChangeAVGUpdateTime(stopwatch.Elapsed.TotalSeconds);
            String avgString = this.avgTimeToUpdate.ToString();
            if (avgString.Length >= 9)
            {
                avgString = this.avgTimeToUpdate.ToString().Substring(0, 9);
            }
            return "Took " + stopwatch.Elapsed.TotalSeconds + " seconds to update " + Ships.Count + " ships, without threading. AVG: " + avgString;
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
                            dockingStation.LeaveShip();
                            _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
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

        public void ChangeAVGUpdateTime(double timeToUpdate)
        {
            this.amountOfUpdates += 1.0;
            double totalTimeToUpdateBefore = this.avgTimeToUpdate * (amountOfUpdates - 1);
            double totalTimeToUpdate = totalTimeToUpdateBefore + timeToUpdate;
            double avgTimeToUpdate0 = totalTimeToUpdate / this.amountOfUpdates;
            this.avgTimeToUpdate = avgTimeToUpdate0;
        }

        public List<String> ManageWarehouse()
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
