using HarborUWP.Models.Commands;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
        public void updateUI(String result)
        {
            application.executeCommand(new UpdateUICommand(result));
        }
        private void clearEventLogButton_Click(object sender, RoutedEventArgs e)
        {
            application.executeCommand(new ClearEventLogCommand());
        }
    }
}
