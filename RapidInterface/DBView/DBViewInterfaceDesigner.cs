using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;

namespace RapidInterface
{
    public class DBViewInterfaceDesigner : ControlDesigner
    {
        DBViewInterface dbInterfaceView;

        /*
        DesignerVerbCollection verbs;
        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (verbs == null)
                    verbs = new DBInterfaceViewDesignerVerbCollections(dbInterfaceView);
                return verbs;
            }
        }
         */

        public override void Initialize(System.ComponentModel.IComponent component)
        {
            base.Initialize(component);
            dbInterfaceView = Control as DBViewInterface;
        }


        public override void InitializeNewComponent(System.Collections.IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
        }
    }
}
