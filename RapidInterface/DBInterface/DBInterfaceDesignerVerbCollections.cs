using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Design;

namespace RapidInterface
{
    class DBInterfaceDesignerVerbCollections : DesignerVerbCollection
    {
        public DBInterfaceDesignerVerbCollections()
        {            
        }        
        
        DBInterface DBInterface { get; set; }

        public DBInterfaceDesignerVerbCollections(DBInterface dbInterface)
        {
            DBInterface = dbInterface;

            Add(new DesignerVerb("Run Designer", OnDesigner));
        }

        public DBInterfaceDesignerVerbCollections(DesignerVerb[] value)
            : base(value)
        {
            
        }
         

        public void OnDesigner(object sender, EventArgs e)
        {
            DBInterface.ShowDesigner();
        }
    }
}
