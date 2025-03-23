using System;
using System.Collections.Generic;
using System.Text;

namespace BilderalbumCS
{
    public class ViewerDoc
    {

        public event OnFileAddedEventHandler EventFileAdded;
        public delegate void OnFileAddedEventHandler(object sender, EventArgs e);
        protected virtual void OnFileAdded()
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            OnFileAddedEventHandler handler = EventFileAdded;
            EventArgs e = new EventArgs();
            // Event will be null if there are no subscribers
            if (handler != null)
            {
                // Use the () operator to raise the event.
                handler(this, e);
            }
        }
        public void ConnectToEvent(OnFileAddedEventHandler Handler)
        {
            EventFileAdded += Handler;
        }
    }
}
