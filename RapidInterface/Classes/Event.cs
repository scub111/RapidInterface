using DevExpress.Xpo;
using System;

namespace RapidInterface
{
    public class Event
    {
        public class CurrentObjectEventArgs : EventArgs
        {
            public CurrentObjectEventArgs(XPBaseObject currentObject)
            {
                CurrentObject = currentObject;
            }

            public XPBaseObject CurrentObject { get; private set; }
        }
    }
}
