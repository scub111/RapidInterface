using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Drawing.Design;


namespace RapidInterface
{
    class DBFormActionList : DesignerActionList
    {
        // Конструктор
        public DBFormActionList(IComponent component)
            : base(component)
        {
            // Сохраняем ссылку на редактируемый компонент
            DBForm = component as DBForm;
            // Сохраняем ссылку на ActionList сервис
            designerActionUIService = GetService(typeof(DesignerActionUIService)) as DesignerActionUIService;
        }

        DesignerActionUIService designerActionUIService;

        /// <summary>
        /// Главный компонент дизайна.
        /// </summary>
        public DBForm DBForm { get; set; }


        [Editor(typeof(DBFormItemCollectionEditor), typeof(UITypeEditor))]
        public DBFormItemBases Items
        {
            get { return DBForm.Items; }
        }

        public void OnDesigner()
        {
            DBForm.ShowDesigner();
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();

            items.Add(new DesignerActionHeaderItem("Properties", "Properties"));

            items.Add(new DesignerActionPropertyItem("Items",
                             "Items", "Properties",
                             "Коллекция элементов"));

            //-------------------------------
            items.Add(new DesignerActionHeaderItem("Methods", "Methods"));

            items.Add(new DesignerActionMethodItem(this, "OnDesigner",
                             "Run Designer", "Methods",
                             "Show designer"));

            return items;
        }
    }
}
