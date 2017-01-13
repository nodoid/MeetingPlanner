using System;
namespace MeetingPlanner.iOS
{
    public class CheckedChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs"/> class.
        /// </summary>
        /// <param name="value">Value of the argument</param>
        public CheckedChangedEventArgs(bool chkd)
        {
            Checked = chkd;
        }

        /// <summary>
        /// Gets the value of the event argument
        /// </summary>
        public bool Checked { get; private set; }
    }
}
