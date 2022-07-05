using System;
using Windows.UI.Popups;

namespace HarborUWP.Models.Commands
{
    internal class PauseCommand : ICommand
    {
        public async void Execute(Application application)
        {
            if (application.controller.Harbor == null)
            {
                string message = "No harbor has been created yet";
                var dialog = new MessageDialog(message);
                await dialog.ShowAsync();
                return;
            }

            if (application.isPaused)
            {
                application.controller.ContinueSimulation();
                application.mainPage.pauseButton.Content = "Pause Simulation";
                application.isPaused = false;
            }
            else
            {
                application.controller.StopSimulation();
                application.mainPage.pauseButton.Content = "Continue Simulation";
                application.isPaused = true;
            }


            application.controller.RunThreaded = (bool)application.mainPage.runTreadedCheckBox.IsChecked;
        }
    }
}
