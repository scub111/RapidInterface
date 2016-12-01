using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using DevExpress.Xpo;


namespace RapidInterface
{
    class DBInterfaceActionList : DesignerActionList
    {
        // Конструктор
        public DBInterfaceActionList(IComponent component)
            : base(component)
        {
            // Сохраняем ссылку на редактируемый компонент
            DBInterface = component as DBInterface;
            // Сохраняем ссылку на ActionList сервис
            designerActionUIService = GetService(typeof(DesignerActionUIService)) as DesignerActionUIService;
        }

        DesignerActionUIService designerActionUIService;

        /// <summary>
        /// Главный компонент дизайна.
        /// </summary>
        public DBInterface DBInterface { get; set; }

        [TypeConverter(typeof(XPSourceConverter<XPBaseObject>))]
        public Type TableType
        {
            get
            {
                return DBInterface.TableType;
            }
            set
            {
                DBInterface.TableType = value;
            }
        }

        public DockStyle Dock
        {
            get { return DBInterface.Dock; }
            set
            {
                DBInterface.Dock = value;
                designerActionUIService.Refresh(DBInterface);       
            }
        }

        [Editor(typeof(DBInterfaceItemCollectionEditor), typeof(UITypeEditor))]
        public DBInterfaceItemBases Items
        {
            get { return DBInterface.Items; }
        }

        /// <summary>
        /// Коллекция XPCollection, с которыми работает основной компонент.
        /// </summary>
        public XPCollectionContainer XPCollections
        {
            get { return DBInterface.XPCollections; }
        }

        public void OnDesigner()
        {
            DBInterface.ShowDesigner();
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();

            items.Add(new DesignerActionHeaderItem("Properties", "Properties"));

            items.Add(new DesignerActionPropertyItem("TableType",
                             "TableType", "Properties",
                             "Тип таблицы"));

            items.Add(new DesignerActionPropertyItem("Dock",
                             "Dock", "Properties",
                             "Расположение"));

            items.Add(new DesignerActionPropertyItem("Items",
                             "Items", "Properties",
                             "Коллекция элементов"));

            items.Add(new DesignerActionPropertyItem("XPCollections",
                             "XPCollections", "Properties",
                             "Коллекция XPCollection"));
            
            //-------------------------------
            items.Add(new DesignerActionHeaderItem("Methods", "Methods"));

            items.Add(new DesignerActionMethodItem(this, "OnDesigner",
                             "Run Designer", "Methods",
                             "Show designer"));

            //-------------------------------
            items.Add(new DesignerActionHeaderItem("Information", "Info"));

            string info = string.Format("Размер {0}x{1}", DBInterface.Width, DBInterface.Height);
            items.Add(new DesignerActionTextItem(info, "Info"));

            return items;
        }

        // Возвращает дескриптор свойства по имени
        private PropertyDescriptor GetPropertyByName(String propName)
        {
            PropertyDescriptor prop = TypeDescriptor.GetProperties(DBInterface)[propName];
            if (prop == null)
                throw new ArgumentException("Свойство не существует", propName);
            return prop;
        }         
    }
}
