using HarborUWP.Models;
using HarborUWP.Models.Ships;
using HarborUWP.Models.Ships.ShipFactory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using Windows.UI.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HarborUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private bool isPaused;
        private Controller controller;
        private TimeStamp timeStamp;

        public MainPage()
        {
            this.InitializeComponent();
            controller = new Controller();
            controller.setMainPage(this);
            isPaused = false;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void newHarborButton_Click(object sender, RoutedEventArgs e)
        {
            controller.runThreaded = (bool)runTreadedCheckBox.IsChecked;
            controller.Initialize();
            eventLogListBox.Items.Clear();
            timeStamp = new TimeStamp(1, 0, 23);
        }

        private async void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (controller.Harbor == null)
            {
                string message = "No harbor has been created yet";
                var dialog = new MessageDialog(message);
                await dialog.ShowAsync();
                return;
            }

            if (isPaused)
            {
                controller.ContinueSimulation();
                this.pauseButton.Content = "Pause Simulation";
                this.isPaused = false;
            }
            else
            {
                controller.StopSimulation();
                this.pauseButton.Content = "Continue Simulation";
                this.isPaused = true;
            }
            

            controller.runThreaded = (bool)runTreadedCheckBox.IsChecked;

           /* if (timeStamp.IsStartDay())
            {
                eventLogListBox.Items.Add(timeStamp.StartDay());
            }

            for (int i = 0; i < 60; i++)
            {
                List<string> stringList = controller.UpdateShips();
                for (int j = 0; j < stringList.Count; j++)
                {
                    string minuteString = i.ToString();
                    if (i < 10)
                    {
                        minuteString = "0" + i;
                    }
                    eventLogListBox.Items.Add($"Day {timeStamp.DayCount} [{timeStamp.CurrHour}:{minuteString}] {stringList[j]}");
                }
            }

            timeStamp.UpdateHour();

            if (timeStamp.IsEndOfDay())
            {
                eventLogListBox.Items.Add(timeStamp.EndDay());
            }*/
        }

        public async void tmr_Elapsed(object sender, EventArgs e)
        {
            foreach (String log in controller.UpdateShips())
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    eventLogListBox.Items.Add(log);
                });
                //Debug.WriteLine(log);
            }
        }

        private void clearEventLogButton_Click(object sender, RoutedEventArgs e)
        {
            eventLogListBox.Items.Clear();
        }
    }
}
