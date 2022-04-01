using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborUWP.Models.Commands
{
    internal interface ICommand
    {
        void Execute(Models.Application application);
    }
}
