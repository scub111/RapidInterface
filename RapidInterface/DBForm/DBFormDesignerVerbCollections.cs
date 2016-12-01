using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Design;

namespace RapidInterface
{
    class DBFormDesignerVerbCollections : DesignerVerbCollection
    {
        public DBFormDesignerVerbCollections()
        {
            DBForm = null;
        }

        DBForm DBForm { get; set; }

        public DBFormDesignerVerbCollections(DBForm dbForm)
        {
            DBForm = dbForm;

            Add(new DesignerVerb("Run Designer", OnDesigner));
        }

        public DBFormDesignerVerbCollections(DesignerVerb[] value)
            : base(value)
        {
            
        }
         

        public void OnDesigner(object sender, EventArgs e)
        {
            DBForm.ShowDesigner();
        }
    }
}
