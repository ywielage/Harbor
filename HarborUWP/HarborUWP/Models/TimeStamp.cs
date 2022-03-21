using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborUWP.Models
{
    class TimeStamp
    {
        public int DayCount { get; set; }
        public int CurrHour { get; set; }
        public int MinHour { get; set; }
        public int MaxHour { get; set; }

        public TimeStamp(int dayCount, int minHour, int maxHour)
        {
            this.DayCount = dayCount;
            this.CurrHour = minHour;
            this.MinHour = minHour;
            this.MaxHour = maxHour;
        }

        public bool IsStartDay()
        {
            return CurrHour == MinHour;
        }

        public String StartDay()
        {
            return $"Day {DayCount} starts";
        }

        public bool IsEndOfDay()
        {
            return CurrHour > MaxHour;
        }

        public String EndDay()
        {
            String returnString = $"Day {DayCount} ends";
            CurrHour = MinHour;
            DayCount++;
            return returnString;
        }

        public void UpdateHour()
        {
            CurrHour++;
        }
    }
}
