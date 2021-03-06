using System;

namespace HarborUWP.Models.Exceptions
{
    internal class UpdateTookLongerThanTimerException : Exception
    {
        public UpdateTookLongerThanTimerException(double timeToUpdate, double timerTime)
        : base(String.Format("The update took: " + timeToUpdate + " seconds Which is longer than the timer: " + timerTime / 1000 + " seconds" +
            "\n This is because the system is too slow to update everything within the timespan of the timer " +
            "\n Try to increase timerTimeInMs in the controller class"))
        {

        }
    }
}
