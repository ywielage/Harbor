using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborUWP.Models
{
    internal class TimeUntilDone
    {
        public int DurationInMins { get; set; }

        public TimeUntilDone(int durationInMins)
        {
            DurationInMins = durationInMins;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
