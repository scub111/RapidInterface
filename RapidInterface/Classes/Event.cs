using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
