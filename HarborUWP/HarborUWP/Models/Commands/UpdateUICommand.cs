using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            application.controller.StopSimulation();
            await application.mainPage.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                stopwatch.Stop();
                Debug.WriteLine(stopwatch.ElapsedMilliseconds);
                stopwatch.Restart();
                //application.mainPage.eventLogListBox.ItemsSource = result;
                //application.mainPage.eventLogListBox.SelectedIndex = application.mainPage.eventLogListBox.Items.Count - 1;
                //application.mainPage.eventLogListBox.ScrollIntoView(application.mainPage.eventLogListBox.SelectedItem);
                application.dockingStationView.UpdateDockingStationsStackPanel(application.controller.Harbor.DockingStations, application.mainPage.dockingStationStackPanel);
            });
            stopwatch.Stop();
            Debug.WriteLine(stopwatch.ElapsedMilliseconds);
            application.controller.ContinueSimulation();
        }
    }
}
