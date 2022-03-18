using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborApp.Models.Enums
{
    internal enum State
    {
        InOpenWaters,
        WaitingInPortWaters,
        Docking,
        Leaving,
        Loading,
        Offloading
    }
}
