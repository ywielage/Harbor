using HarborUWP.Models;
using HarborUWP.Models.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborUWP.Models
{

    internal class Application
    {
        public MainPage mainPage { get; set; }
        public bool isPaused { get; set; }
        public Controller controller { get; set; }
        public DockingStationView dockingStationView { get; set; }
        public ShipStateTable shipStateTable { get; set; }
        public WarehouseStateTable warehouseStateTable { get; set; }

        public Application(MainPage mainPage)
        {
            this.mainPage = mainPage;
            controller = new Controller();
            controller.setMainPage(mainPage);
            dockingStationView = new DockingStationView();
            shipStateTable = new ShipStateTable();
            warehouseStateTable = new WarehouseStateTable();
            isPaused = false;
        }
        public void executeCommand(ICommand command)
        {
            command.Execute(this);
        }

    }
}
