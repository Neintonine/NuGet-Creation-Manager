using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NUGETManager.Storage
{
    [Serializable]
    public class EventList : ObservableCollection<EventObject>
    {
        public IEnumerable<EventObject> this[EventType type]
        {
            get
            {
                return this.Where(a => a.EventType == type);
            }
        }
    }
}