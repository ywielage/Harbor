namespace HarborUWP.Models.Commands
{
    internal class ClearEventLogCommand : ICommand
    {
        public void Execute(Application app)
        {
            app.mainPage.eventLogTextBlock.Text = "";
        }
    }
}
