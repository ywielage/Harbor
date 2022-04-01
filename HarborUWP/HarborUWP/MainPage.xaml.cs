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
using Microsoft.Toolkit.Uwp.UI.Controls;
using HarborUWP.Models.Commands;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HarborUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Models.Application application { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            this.application = new Models.Application(this);
        }
        private void newHarborButton_Click(object sender, RoutedEventArgs e)
        {
            application.executeCommand(new NewHarborCommand());
        }
        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            application.executeCommand(new PauseCommand());
        }
        public void updateUI(List<String> result)
        {
            application.executeCommand(new UpdateUICommand(result));  
        }
        private void clearEventLogButton_Click(object sender, RoutedEventArgs e)
        {
            application.executeCommand(new ClearEventLogCommand());
        }
    }
}
