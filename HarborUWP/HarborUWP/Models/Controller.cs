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

namespace HarborUWP.Models
{
    internal class Controller
    {
        public Harbor Harbor { get; set; }
        public List<Ship> Ships { get; set; }
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
            this.timer = new System.Timers.Timer();
            //Interval in ms
            this.timer.Interval = 10000;
            this.timer.Elapsed += tmr_Elapsed;
            this.timer.Start();
        }

        public void tmr_Elapsed(object sender, EventArgs e)
        {
            foreach (String log in UpdateShips())
            {
                //update front end log with new log entries
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
            //maak een stopwatch om te timen
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //een lijst om de return waardes van alle ships op te halen
            List<String> returnList = new List<String>();

            //loop door alle ships
            foreach (Ship ship in Ships)
            {
                //maak een update task die de update in de ships calt
                WaitCallback updateTask = (aShip) =>
                {
                    //haal het megegeven ship op om te gebruiken
                    Ship selectedShip = aShip as Ship;

                    //roep de ships update aan en sla het resultaat op
                    Thread.Sleep(50);//voor test, komt later in ships update method
                    String result = selectedShip.Update();
                    //String result = "Threaded ship updated " + selectedShip.Id + " " + selectedShip.GetType();

                    //lock de lijst om te zorgen dat ze niet tegelijk dingen toevoegen
                    lock (returnList)
                    {
                        //voeg het resultaat toe aan de lijst
                        returnList.Add(result);
                    } 
                };

                //voeg de updateTask toe met een ship uit de loop
                ThreadPool.QueueUserWorkItem(updateTask, ship);
            };

            while (true)
            {
                //zorg ervoor dat hij alleen stopt als hij klaar is met alle ships
                if (returnList.Count == Ships.Count)
                {
                    //stop de stopwatch om te meten of hij klaar is
                    stopwatch.Stop();
                    String timeToUpdate = stopwatch.Elapsed.TotalSeconds.ToString();
                    //voeg een string oveer de stopwatch terug aan de returnList
                    returnList.Add("Took " + timeToUpdate + " seconds to update " + Ships.Count + " ships");
                    return returnList;
                }
            }
        }

        private List<String> UpdateShipsNonThreaded()
        {
            //maak een stopwatch om te timen
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //een lijst om de return waardes van alle ships op te halen
            List<String> returnList = new List<String>();

            //loop door alle ships
            foreach (Ship ship in Ships)
            {
                //roep de ships update aan en sla het resultaat op
                Thread.Sleep(50);//voor test, komt later in ships update method
                returnList.Add(ship.Update());
                //returnList.Add("Non Threaded ship updated" + ship.Id + ship.GetType());
            }
            //stop de stopwatch om te meten of hij klaar is
            stopwatch.Stop();
            String timeToUpdate = stopwatch.Elapsed.TotalSeconds.ToString();
            //voeg een string oveer de stopwatch terug aan de returnList
            returnList.Add("Took " + timeToUpdate + " seconds to update " + Ships.Count + " ships");
            return returnList;
        }

        #endregion
    }
}
