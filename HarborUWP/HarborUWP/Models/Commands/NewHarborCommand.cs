using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborUWP.Models.Commands
{
    internal class NewHarborCommand : ICommand
    {
        public void Execute(Application application)
        {
            application.controller.runThreaded = (bool)application.mainPage.runTreadedCheckBox.IsChecked;
            application.controller.Initialize();
            application.mainPage.eventLogListBox.Items.Clear();
            application.dockingStationView.Initialize(application.controller.Harbor.DockingStations, application.mainPage.dockingStationStackPanel);
            application.shipStateTable.Initialize(application.mainPage.containerGrid, application.controller.Ships);
            application.warehouseStateTable.Initialize(application.mainPage.containerGrid, application.controller.Harbor.Warehouse);
        }
    }
}
