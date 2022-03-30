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
using Windows.UI.Xaml.Shapes;
using Windows.UI;
using System.Threading.Tasks;

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
        private DockingStationView dockingStationView;

        public MainPage()
        {
            this.InitializeComponent();
            controller = new Controller();
            controller.setMainPage(this);
            dockingStationView = new DockingStationView();
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
            dockingStationView.Initialize(controller.Harbor.DockingStations, dockingStationStackPanel);
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

        public async void updateUI(List<String> result)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.controller.StopSimulation();
            await Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                stopwatch.Stop();
                Debug.WriteLine(stopwatch.ElapsedMilliseconds);
                stopwatch.Restart();
                eventLogListBox.ItemsSource = result;
                eventLogListBox.SelectedIndex = eventLogListBox.Items.Count - 1;
                eventLogListBox.ScrollIntoView(eventLogListBox.SelectedItem);
                dockingStationView.UpdateDockingStationsStackPanel(controller.Harbor.DockingStations, dockingStationStackPanel);
                UpdateShipsListView(controller.Ships);
            });
            stopwatch.Stop();
            Debug.WriteLine(stopwatch.ElapsedMilliseconds);
            this.controller.ContinueSimulation();


        }

        private void clearEventLogButton_Click(object sender, RoutedEventArgs e)
        {
            eventLogListBox.Items.Clear();
        }

        private void UpdateShipsListView(List<Ship> ships)
        {
            shipsListView.Items.Clear();
            foreach (Ship ship in ships)
            {
                //TODO: Solution which doesnt have to create 5 objects for every ship
                StackPanel stackPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    MaxHeight = 20,
                    MinHeight = 20,
                };

                stackPanel.Children.Add(GenerateListViewItem(100, ship.Id.ToString()));
                stackPanel.Children.Add(GenerateListViewItem(220, ship.GetType().ToString().Substring(23)));
                stackPanel.Children.Add(GenerateListViewItem(240, ship.State.ToString()));
                stackPanel.Children.Add(GenerateListViewItem(160, ship.TimeUntilDone.DurationInMins.ToString()));

                shipsListView.Items.Add(stackPanel);
            }
        }

        private ListViewItem GenerateListViewItem(int width, string content)
        {
            return new ListViewItem
            {
                MaxWidth = width,
                MinWidth = width,
                MaxHeight = 20,
                MinHeight = 20,
                Content = content,
                FontSize = 11
            };
        }
    }
}
