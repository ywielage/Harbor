using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace HarborUWP.Models.Commands
{
    internal class NewHarborCommand : ICommand
    {
        public async void Execute(Application application)
        {
            application.controller.RunThreaded = (bool)application.mainPage.runTreadedCheckBox.IsChecked;

            string shipAmount = application.mainPage.amountOfShipsTextBox.Text;
            string dockingStationAmount = application.mainPage.amountOfDockingStationsTextBox.Text;

            if (!int.TryParse(shipAmount, out var realShipAmount))
            {
                string message = "Ship amount is not an integer";
                var dialog = new MessageDialog(message);
                await dialog.ShowAsync();
                return;
            }

            if (!int.TryParse(dockingStationAmount, out var realDockingStationAmount))
            {
                string message = "Docking station amount is not an integer";
                var dialog = new MessageDialog(message);
                await dialog.ShowAsync();
                return;
            }

            await Task.Run(() => application.controller.Initialize(realShipAmount, realDockingStationAmount));
            application.mainPage.newHarborButton.IsEnabled = false;
            application.mainPage.newHarborButton.Content = "Harbor started";
            application.mainPage.eventLogTextBlock.Text = "";
            application.dockingStationView.Initialize(application.controller.Harbor.DockingStations, application.mainPage.dockingStationStackPanel);
            application.shipStateTable.Initialize(application.mainPage.containerGrid, application.controller.Ships);
            application.warehouseStateTable.Initialize(application.mainPage.containerGrid, application.controller.Harbor.Warehouse);
        }
    }
}
