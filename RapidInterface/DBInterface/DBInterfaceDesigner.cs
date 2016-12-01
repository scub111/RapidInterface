using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace RapidInterface
{
    public class DBInterfaceDesigner : ControlDesigner
    {
        /// <summary>
        /// Основной компонент.
        /// </summary>
        DBInterface DBInterface { get; set; }

        DesignerVerbCollection DBInterfaceVerbs { get; set; }

        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (DBInterfaceVerbs == null)
                    DBInterfaceVerbs = new DBInterfaceDesignerVerbCollections(DBInterface);
                return DBInterfaceVerbs;
            }
        }

        DesignerActionListCollection actionLists { get; set; }
        DBInterfaceActionList dbInterfaceActionList { get; set; }
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                // Если еще не создавали actionList
                if (actionLists == null)
                {
                    // Создаем ActionList
                    actionLists = new DesignerActionListCollection();
                    // Добавляем тег
                    dbInterfaceActionList = new DBInterfaceActionList(Component);
                    actionLists.Add(dbInterfaceActionList);
                }

                return actionLists;
            }
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            DBInterface = Control as DBInterface;
            DBInterface.TypeDiscoveryService = (ITypeDiscoveryService)GetService(typeof(ITypeDiscoveryService));
            IComponentChangeService componentChangeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            componentChangeService.ComponentRemoving += componentChangeService_ComponentRemoving;            
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            properties.Remove("BackColor");
        }

        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            DBInterface.InitializeVisibleComponents();
        }

        protected override void Dispose(bool disposing)
        {
            IComponentChangeService componentChangeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            // Желательно улалять обработчк, иначе приходится постоянно переоткрывать дизайнер формы.
            componentChangeService.ComponentRemoving -= componentChangeService_ComponentRemoving;
            base.Dispose(disposing);
        }

        void componentChangeService_ComponentRemoving(object sender, ComponentEventArgs e)
        {
            // If the user is removing the control itself
            if (e.Component == DBInterface)
            {
                IComponentChangeService componentChangeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));
                DBInterface.DestroyVisibleComponents();
                componentChangeService.OnComponentChanged(DBInterface, null, null, null);
            }
        }
    }
}
