using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace HarborUWP.Models.Commands
{
    internal class UpdateCommand : ICommand
    {
        private const int StandardTime = 1300;
        private const int MinTime = 100;

        public UpdateCommand()
        {
        }

        public async void Execute(Application application)
        {
            Stopwatch timer = new Stopwatch();
            //wait so you can see the time needed to initialize everything
            await Task.Delay(2000);
            while (true)
            {
                if (!application.isPaused)
                {
                    timer.Start();
                    var result = application.controller.UpdateShips();
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
