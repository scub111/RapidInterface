using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Design;

namespace RapidInterface
{
    class DBViewInterfaceDesignerVerbCollections : DesignerVerbCollection
    {
        public DBViewInterfaceDesignerVerbCollections()
        {
            DBInterfaceView = null;
        }

        DBViewInterface DBInterfaceView;

        public DBViewInterfaceDesignerVerbCollections(DBViewInterface dbInterfaceView)
        {
            DBInterfaceView = dbInterfaceView;

            Add(new DesignerVerb("Initialize", OnInitialize));
        }

        public DBViewInterfaceDesignerVerbCollections(DesignerVerb[] value)
            : base(value)
        {
            
        }
         

        public void OnInitialize(object sender, EventArgs e)
        {
            DBInterfaceView.InitializeVisibleComponents();
        }
    }
}
