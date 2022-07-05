using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace HarborUWP.Models.Commands
{
    internal class UpdateCommand : ICommand
    {
        private Application application;
        private const int StandardTime = 1300;
        private const int MinTime = 500;

        public UpdateCommand(Application app)
        {
            this.application = app;
        }

        public async void Execute(Application application)
        {
            Stopwatch timer = new Stopwatch();
            while (true)
            {
                if (!this.application.isPaused)
                {
                    timer.Start();
                    var result = this.application.controller.UpdateShips();
                    timer.Stop();
                    var time = (StandardTime - timer.ElapsedMilliseconds >= MinTime) ? StandardTime - timer.ElapsedMilliseconds : MinTime;
                    await Task.Delay((int)(time));
                    timer.Reset();

                    await application.mainPage.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                    {
                        application.mainPage.eventLogTextBlock.Text = result;
                        application.dockingStationView.UpdateDockingStationsStackPanel(
                            application.controller.Harbor.DockingStations,
                            application.mainPage.dockingStationStackPanel);
                    });
                }
                else
                {
                    await Task.Delay(StandardTime);
                }
            }
        }
    }
}
