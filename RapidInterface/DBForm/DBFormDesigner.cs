using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace RapidInterface
{
    public class DBFormDesigner : ControlDesigner
    {
        /// <summary>
        /// Основной компонент.
        /// </summary>
        DBForm DBForm { get; set; }
        
        DesignerActionListCollection actionLists { get; set; }
        DBFormActionList dbFormActionList { get; set; }
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
                    dbFormActionList = new DBFormActionList(Component);
                    actionLists.Add(dbFormActionList);
                }

                return actionLists;
            }
        }
        
        DesignerVerbCollection dbFormVerbs { get; set; }
        
        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (dbFormVerbs == null)
                    dbFormVerbs = new DBFormDesignerVerbCollections(DBForm);
                return dbFormVerbs;
            }
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            DBForm = Control as DBForm;
            DBForm.TypeDiscoveryService = (ITypeDiscoveryService)GetService(typeof(ITypeDiscoveryService));
            IComponentChangeService componentChangeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            componentChangeService.ComponentRemoving += componentChangeService_ComponentRemoving;
        }

        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            object form = defaultValues["Parent"];
            if (form != null && (TypeEx.IsSubclassOf(form.GetType(), typeof(Form)) || form.GetType() ==  typeof(Form)))
            {
                base.InitializeNewComponent(defaultValues);
                DBForm.InitializeVisibleComponents((Form)form);
                DBForm.OwnerForm = form as Form;
            }
            else
            {
                XtraMessageBox.Show("Данный объект не типа Form.",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
                DBForm.DestroyItself();
            }
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
            if (e.Component == DBForm)
            {
                IComponentChangeService componentChangeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));
                DBForm.DestroyVisibleComponents();
                componentChangeService.OnComponentChanged(DBForm, null, null, null);
            }
        }
    }
}
