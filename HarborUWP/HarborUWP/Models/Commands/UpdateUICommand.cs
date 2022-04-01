using System;
using System.Diagnostics;
using Windows.UI.Core;

namespace HarborUWP.Models.Commands
{
    internal class UpdateUICommand : ICommand
    {
        private String result;
        public UpdateUICommand(String result)
        {
            this.result = result;
        }
        public async void Execute(Application application)
        {
            application.controller.StopSimulation();
            await application.mainPage.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                application.mainPage.eventLogTextBlock.Text = result;
                application.dockingStationView.UpdateDockingStationsStackPanel(application.controller.Harbor.DockingStations, application.mainPage.dockingStationStackPanel);
            });
            application.controller.ContinueSimulation();
        }
    }
}
