using HarborUWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HarborUWP.Models.Commands
{
    internal class ClearEventLogCommand : ICommand
    {
        public void Execute(Application app)
        {
            app.mainPage.eventLogListBox.Items.Clear();
        }
    }
}
