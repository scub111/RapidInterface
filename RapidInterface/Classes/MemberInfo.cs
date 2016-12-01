using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.ObjectModel;
using DevExpress.XtraTreeList;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.XtraTreeList.Nodes;

namespace RapidInterface
{
    #region PropertyInfoEx
    public class PropertyInfoEx
    {
        public PropertyInfoEx(PropertyInfo prInfo)
        {
            Owner = null;
            Parent = null;
            PropertyInfo = prInfo;
            IsXPBaseObject = isXPBaseObject(PropertyInfo.PropertyType);
            IsXPCollection = false;
        }

        public PropertyInfoEx(PropertyInfo prInfo, PropertyInfo prInfoCollection)
        {
            Owner = null;
            Parent = null;
            PropertyInfo = prInfo;
            PropertyInfoCollection = prInfoCollection;
            IsXPBaseObject = false;
            IsXPCollection = true;
        }

        /// <summary>
        /// Владелец-коллекция.
        /// </summary>
        public PropertyInfoExs Owner { get; set; }

        /// <summary>
        /// Родительский объект.
        /// </summary>
        public PropertyInfoEx Parent { get; set; }

        /// <summary>
        /// Тип.
        /// </summary>
        public PropertyInfo PropertyInfo { get; set; }

        /// <summary>
        /// Тип  для коллекции.
        /// </summary>
        public PropertyInfo PropertyInfoCollection { get; set; }

        /// <summary>
        /// Принадлежность к XPObject.
        /// </summary>
        public bool IsXPBaseObject { get; set; }

        /// <summary>
        /// Принадлежность к коллекции.
        /// </summary>
        public bool IsXPCollection { get; set; }

        /// <summary>
        /// Принадлежит ли тип к XPObject.
        /// </summary>
        public static bool isXPBaseObject(Type type)
        {
            if (TypeEx.IsSubclassOf(type, typeof(XPBaseObject)) || type == typeof(XPBaseObject))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Принадлежит ли тип к XPObject.
        /// </summary>
        public static bool isXPBaseObject(TableMemberInfo tmInfo)
        {

            if (!tmInfo.IsXPCollection && isXPBaseObject(tmInfo.PropertyType))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Принадлежит ли тип к XPObject.
        /// </summary>
        public static bool isXPBaseObject(PropertyInfoEx prInfo)
        {
            if (!prInfo.IsXPCollection && isXPBaseObject(prInfo.PropertyInfo.PropertyType))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Принадлежит ли тип к isXPCollection.
        /// </summary>
        public static bool isXPCollection(Type type)
        {
            if (TypeEx.IsSubclassOf(type, typeof(XPBaseCollection)) || type == typeof(XPCollection))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Принадлежил ли тип к XPObject или isXPCollection.
        /// </summary>
        public static bool isXPComplex(Type type)
        {
            if (isXPBaseObject(type) || isXPCollection(type))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Если ли аттрибут Association.
        /// </summary>
        public static string GetAssociationAttribute(PropertyInfoEx info)
        {
            DevExpress.Xpo.AssociationAttribute[] associations =
                info.PropertyInfo.GetCustomAttributes(typeof(AssociationAttribute), false) as DevExpress.Xpo.AssociationAttribute[];
            if (associations.Length > 0)
                return associations[0].Name;
            return "";
        }

        /// <summary>
        /// Получение надписи аттрибута DisplayName.
        /// </summary>
        public static string GetDisplayNameAttribute(PropertyInfo prInfo)
        {
            DevExpress.Xpo.DisplayNameAttribute[] captions =
                prInfo.GetCustomAttributes(typeof(DevExpress.Xpo.DisplayNameAttribute), false) as DevExpress.Xpo.DisplayNameAttribute[];
            if (captions.Length > 0)
                return captions[0].DisplayName;
            return "";
        }
    }
    #endregion

    #region PropertyInfoExs
    public class PropertyInfoExs : Collection<PropertyInfoEx>
    {
        public PropertyInfoExs(PropertyInfoEx parent = null)
        {
            Parent = parent;
        }

        /// <summary>
        /// Родительский объект.
        /// </summary>
        public PropertyInfoEx Parent { get; set; }

        /// <summary>
        /// Перегрузка на добавление новой записи.
        /// </summary>
        protected override void InsertItem(int index, PropertyInfoEx item)
        {
            item.Parent = Parent;
            item.Owner = this;
            base.InsertItem(index, item);
        }
    }
    #endregion

    #region TableMemberInfo
    /// <summary>
    /// Класс для отображение записей в таблице дизайнера DBInterface.
    /// </summary>
    public class TableMemberInfo
    {
        public TableMemberInfo()
        {
            _Items = new TableMemberInfos(this);
            IsXPBaseObject = false;
            IsXPCollection = false;
            Item = null;
            Owner = null;
            Parent = null;
            IsUsed = false;
            ImageIndex = -1;
            _HashCode = GetHashCode();
        }

        public TableMemberInfo(string field, Type memberType)
            : this()
        {
            FieldName = field;
            PropertyType = memberType;
        }

        public TableMemberInfo(string fieldName, Type propertyType = null, string typeName = "", string caption = "",
            bool isXPCollection = false, DBInterfaceItemBase dbItem = null, bool isUsed = false)
            : this()
        {
            FieldName = fieldName;
            PropertyType = propertyType;
            TypeName = typeName;
            Caption = caption;
            IsXPCollection = isXPCollection;
            Item = dbItem;
            IsUsed = isUsed;
        }

        /// <summary>
        /// Владелец-коллекция.
        /// </summary>
        public TableMemberInfos Owner { get; set; }

        /// <summary>
        /// Родительский объект.
        /// </summary>
        public TableMemberInfo Parent { get; set; }

        string _FieldName;
        /// <summary>
        /// Название поля.
        /// </summary>
        public string FieldName
        {
            get { return _FieldName; }
            set
            {
                if (_FieldName == value) return;
                _FieldName = value;
                OnChanged();
                if (Item != null)
                    Item.FieldName = value;
            }
        }

        Type _PropertyType;
        /// <summary>
        /// Тип поля.
        /// </summary>
        public Type PropertyType
        {
            get { return _PropertyType; }
            set
            {
                if (_PropertyType == value) return;
                _PropertyType = value;
                OnChanged();
            }
        }

        /// <summary>
        /// Представление типа поля.
        /// </summary>
        public string PropertyTypeName
        {
            get
            {
                if (IsXPCollection)
                    return string.Format("XPC<{0}>", PropertyTypeCollection.Name);
                else
                    return PropertyType.Name;
            }
        }

        Type _PropertyTypeCollection;
        /// <summary>
        /// Тип поля для коллекции.
        /// </summary>
        public Type PropertyTypeCollection
        {
            get { return _PropertyTypeCollection; }
            set
            {
                if (_PropertyTypeCollection == value) return;
                _PropertyTypeCollection = value;
                OnChanged();
            }
        }

        string _TypeName;
        /// <summary>
        /// Название типа поля.
        /// </summary>
        public string TypeName
        {
            get
            {
                return _TypeName;
            }
            set
            {
                if (_TypeName == value) return;
                _TypeName = value;
                OnChanged();
            }
        }

        string _Caption;
        /// <summary>
        /// Название поля.
        /// </summary>
        public string Caption
        {
            get { return _Caption; }
            set
            {
                if (_Caption == value) return;
                _Caption = value;
                OnChanged();
                if (Item != null)
                    Item.Caption = value;
            }
        }

        TableMemberInfos _Items;
        /// <summary>
        /// Список дочерних записей.
        /// </summary>
        public TableMemberInfos Items
        {
            get { return _Items; }
        }

        /// <summary>
        /// Принадлежность к коллекции.
        /// </summary>
        public bool IsXPCollection { get; set; }

        /// <summary>
        /// Принадлежность к IsXPBaseObject.
        /// </summary>
        public bool IsXPBaseObject { get; set; }

        bool _IsUsed;
        /// <summary>
        /// Используется это свойство в уже заданное для таблице.
        /// </summary>
        public bool IsUsed
        {
            get { return _IsUsed; }
            set
            {
                if (_IsUsed == value) return;
                _IsUsed = value;
                OnChanged();
            }
        }

        int _ImageIndex;
        /// <summary>
        /// Индекс иконки.
        /// </summary>
        public int ImageIndex
        {
            get { return _ImageIndex; }
            set
            {
                if (_ImageIndex == value) return;
                _ImageIndex = value;
                OnChanged();           
                if (Item != null)
                    Item.ImageIndex = value;
            }
        }

        /// <summary>
        /// Название иконки.
        /// </summary>
        public string ImageName { get; set; }

        DBInterfaceItemBase _Item;
        /// <summary>
        /// Привязанная уже созданная запись.
        /// </summary>
        public DBInterfaceItemBase Item
        {
            get { return _Item; }
            set
            {
                if (_Item == value) return;
                _Item = value;
                if (_Item != null)
                    _Item.PropertyChanged += new EventHandler(ItemBase_PropertyChanged);
            }
        }

        int _HashCode;
        /// <summary>
        /// Hash-код.
        /// </summary>
        public int HashCode
        {
            get
            {
                return _HashCode;
            }
        }

        /// <summary>
        /// Обработчик события на изменение свойств ItemBase.
        /// </summary>
        void ItemBase_PropertyChanged(object sender, EventArgs e)
        {
            DBInterfaceItemBase item = sender as DBInterfaceItemBase;
            if (item != null)
            {
                FieldName = item.FieldName;
                Caption = item.Caption;
                TypeName = item.TypeName;
                ImageIndex = item.ImageIndex;
            }
        }

        /// <summary>
        /// Добавление дочерней записи.
        /// </summary>
        public void Add(TableMemberInfo item)
        {
            Items.Add(item);
        }

        /// <summary>
        /// Метод для динамического обновления элементов, привязанных к этому классу.
        /// </summary>
        void OnChanged()
        {
            if (Owner == null) return;
            int index = Owner.IndexOf(this);
            if (index != -1)
                Owner.ResetItem(index);
        }

        /// <summary>
        /// Поиск самого старшего родителя записи.
        /// </summary>
        public TableMemberInfo GetParent(int limit = 9999)
        {
            return GetParent(this, 1, limit);
        }

        /// <summary>
        /// Рекурсивный метод поиска старшего родителя записи.
        /// </summary>
        private TableMemberInfo GetParent(TableMemberInfo info, int level, int limit = 9999)
        {
            if (level < limit)
            {
                if (info.Parent != null)
                    return GetParent(info.Parent, level + 1, limit);
                else
                    return info;
            }
            else
                return info;
        }

        /// <summary>
        /// Метод определения уровня положения записи.
        /// </summary>
        public int GetLevel()
        {
            return GetLevel(this, 1);
        }

        /// <summary>
        /// Рекурсивный метод определения уровня положения записи.
        /// </summary>
        private int GetLevel(TableMemberInfo info, int level)
        {
            if (info.Parent != null)
                return GetLevel(info.Parent, level + 1);
            else
                return level;
        }
        
        /// <summary>
        /// Порядковый номер в списке.
        /// </summary>
        public int IndexOf()
        {
            return Owner.IndexOf(this);
        }
    }
    #endregion

    #region TableMemberInfos
    /// <summary>
    /// Класс-коллекция для отображение записей в таблице дизайнера DBInterface.
    /// </summary>
    public class TableMemberInfos : BindingList<TableMemberInfo>, TreeList.IVirtualTreeListData
    {
        public TableMemberInfos(TableMemberInfo parent = null, bool rewriteParent = true)
        {
            Parent = parent;
            RewriteParent = rewriteParent;
        }

        /// <summary>
        /// Родительский объект.
        /// </summary>
        public TableMemberInfo Parent { get; set; }

        /// <summary>
        /// Перезапись родительского свойства при добавлении нового элемента
        /// </summary>
        public bool RewriteParent { get; set; }

        /// <summary>
        /// Поиск схожей записи по свойству FieldName в коллекции.
        /// </summary>
        public TableMemberInfo FindInfo(TableMemberInfo info)
        {
            TableMemberInfo infoTemp = null;
            if (info != null)
            {
                TableMemberInfo infoParent = null;
                int level = info.GetLevel();

                TableMemberInfos infosTemp = this;

                for (int i = 0; i < level; i++)
                {
                    infoTemp = null;
                    infoParent = info.GetParent(level - i);

                    foreach (TableMemberInfo infoIter in infosTemp)
                    {
                        if (infoIter.FieldName == infoParent.FieldName)
                        {
                            infoTemp = infoIter;
                            break;
                        }
                    }
                    if (infoTemp != null)
                        infosTemp = infoTemp.Items;
                    else
                        break;
                }
            }
            return infoTemp;
        }

        /// <summary>
        /// Очистка записи таблицы.
        /// </summary>
        public void RemoveEx(TableMemberInfo info)
        {
            if (info.Parent != null)
                info.Parent.Items.Remove(info);
            else
                Remove(info);
        }

        /// <summary>
        /// Перегрузка на добавление новой записи.
        /// </summary>
        protected override void InsertItem(int index, TableMemberInfo item)
        {
            if (RewriteParent)
            {
                item.Parent = Parent;
                item.Owner = this;
            }
            base.InsertItem(index, item);
        }

        #region IVirtualTreeListData Members
        public void VirtualTreeGetChildNodes(VirtualTreeGetChildNodesInfo info)
        {
            TableMemberInfo obj = info.Node as TableMemberInfo;
            info.Children = obj.Items;
        }

        public void VirtualTreeGetCellValue(VirtualTreeGetCellValueInfo info)
        {
            TableMemberInfo tmInfo = info.Node as TableMemberInfo;
            switch (info.Column.FieldName)
            {
                case "FieldName":
                    info.CellData = tmInfo.FieldName;
                    break;
                case "PropertyType":
                    if (tmInfo.PropertyType != null)
                        info.CellData = tmInfo.PropertyType.Name.ToString();
                    else
                        info.CellData = "null";
                    break;
                case "PropertyTypeName":
                    info.CellData = tmInfo.PropertyTypeName;
                    break;
                case "TypeName":
                    info.CellData = tmInfo.TypeName;
                    break;
                case "Caption":
                    info.CellData = tmInfo.Caption;
                    break;
                case "IsXPBaseObject":
                    info.CellData = tmInfo.IsXPBaseObject;
                    break;
                case "IsXPCollection":
                    info.CellData = tmInfo.IsXPCollection;
                    break;
                case "IsUsed":
                    info.CellData = tmInfo.IsUsed;
                    break;
                case "ImageIndex":
                    info.CellData = tmInfo.ImageIndex;
                    break;
                case "HashCode":
                    info.CellData = tmInfo.HashCode;
                    break;
            }
        }

        public void VirtualTreeSetCellValue(VirtualTreeSetCellValueInfo info)
        {
            TableMemberInfo tmInfo = info.Node as TableMemberInfo;
            switch (info.Column.FieldName)
            {
                case "FieldName":
                    tmInfo.FieldName = (string)info.NewCellData;
                    break;
                case "Caption":
                    tmInfo.Caption = (string)info.NewCellData;
                    break;
                    /*
                case "IsCollection":
                    tmInfo.IsXPCollection = (bool)info.NewCellData;
                    break;
                case "IsUsed":
                    tmInfo.IsUsed = (bool)info.NewCellData;
                    break;
                     */
                case "ImageIndex":
                    tmInfo.ImageIndex = int.Parse((string)info.NewCellData);
                    break;
            }
        }
        #endregion
    }
    #endregion
    
    #region ViewMemberInfo
    /// <summary>
    /// Класс для отображение возможных представлений для дизайнера DBForm
    /// </summary>
    public class ViewMemberInfo
    {
        public ViewMemberInfo()
        {
            _Items = new ViewMemberInfos();
            _HashCode = GetHashCode();
            _IsUsed = false;
        }

        /// <summary>
        /// Владелец-коллекция.
        /// </summary>
        public ViewMemberInfos Owner { get; set; }

        ViewMemberInfos _Items;
        /// <summary>
        /// Список дочерних записей.
        /// </summary>
        public ViewMemberInfos Items
        {
            get { return _Items; }
        }

        Type _ViewType;
        /// <summary>
        /// Тип поля.
        /// </summary>
        public Type ViewType
        {
            get { return _ViewType; }
            set
            {
                if (_ViewType == value) return;
                _ViewType = value;
                OnChanged();
                if (Item != null)
                    Item.ViewType = value;
            }
        }

        /// <summary>
        /// Тип интерфейса.
        /// </summary>
        public string ViewTypeName
        {
            get
            {
                if (ViewType != null)
                    return ViewType.Name;
                else
                    return "[null]";
            }
        }

        /*
        Type _TableType;
        /// <summary>
        /// Тип таблицы.
        /// </summary>
        public Type TableType
        {
            get { return _TableType; }
            set
            {
                if (_TableType == value) return;
                _TableType = value;
                OnChanged();
                if (Item != null)
                    Item.TableType = value;
            }
        }
         */

        DBFormItemBase _Item;
        /// <summary>
        /// Привязанная уже созданная запись.
        /// </summary>
        public DBFormItemBase Item
        {
            get { return _Item; }
            set
            {
                if (_Item == value) return;
                _Item = value;
                if (_Item != null)
                    _Item.PropertyChanged += new EventHandler(Item_PropertyChanged);
            }
        }

        /// <summary>
        /// Обработчик события на изменение свойств ItemBase.
        /// </summary>
        void Item_PropertyChanged(object sender, EventArgs e)
        {
            DBFormItemBase item = sender as DBFormItemBase;
            if (item != null)
            {
                Caption = item.Caption;
                ImageIndex = item.ImageIndex;
                ViewType = item.ViewType;
                //TableType = item.TableType;
            }
        }

        string _Caption;
        /// <summary>
        /// Название формы.
        /// </summary>
        public string Caption
        {
            get { return _Caption; }
            set
            {
                if (_Caption == value) return;
                _Caption = value;
                OnChanged();
                if (Item != null)
                    Item.Caption = value;
            }
        }

        int _ImageIndex;
        /// <summary>
        /// Индекс иконки.
        /// </summary>
        public int ImageIndex
        {
            get { return _ImageIndex; }
            set
            {
                if (_ImageIndex == value) return;
                _ImageIndex = value;
                OnChanged();
                if (Item != null)
                    Item.ImageIndex = value;
            }
        }

        /// <summary>
        /// Название иконки.
        /// </summary>
        public string ImageName { get; set; }


        int _HashCode;
        /// <summary>
        /// Hash-код.
        /// </summary>
        public int HashCode
        {
            get
            {
                return _HashCode;
            }
        }

        bool _IsUsed;
        /// <summary>
        /// Используется это свойство в уже заданное для таблице.
        /// </summary>
        public bool IsUsed
        {
            get { return _IsUsed; }
            set
            {
                if (_IsUsed == value) return;
                _IsUsed = value;
                OnChanged();
            }
        }

        /// <summary>
        /// Метод для динамического обновления элементов, привязанных к этому классу.
        /// </summary>
        void OnChanged()
        {
            if (Owner == null) return;
            int index = Owner.IndexOf(this);
            if (index != -1)
                Owner.ResetItem(index);
        }
    }
    #endregion

    #region ViewMemberInfos
    /// <summary>
    /// Класс-коллекция для отображение записей в таблице дизайнера DBInterface.
    /// </summary>
    public class ViewMemberInfos : BindingList<ViewMemberInfo>, TreeList.IVirtualTreeListData
    {
        /// <summary>
        /// Перегрузка на добавление новой записи.
        /// </summary>
        protected override void InsertItem(int index, ViewMemberInfo item)
        {
            item.Owner = this;
            base.InsertItem(index, item);
        }

        /// <summary>
        /// Поиск схожей записи по свойству FieldName в коллекции.
        /// </summary>
        public ViewMemberInfo FindInfo(ViewMemberInfo info)
        {
            foreach (ViewMemberInfo infoIter in this)
                if (infoIter.ViewType == info.ViewType)
                    return infoIter;

            return null;
        }

        #region IVirtualTreeListData Members
        public void VirtualTreeGetChildNodes(VirtualTreeGetChildNodesInfo info)
        {
            ViewMemberInfo obj = info.Node as ViewMemberInfo;
            info.Children = obj.Items;
        }

        public void VirtualTreeGetCellValue(VirtualTreeGetCellValueInfo info)
        {
            ViewMemberInfo vwInfo = info.Node as ViewMemberInfo;
            switch (info.Column.FieldName)
            {
                case "ViewTypeName":
                    info.CellData = vwInfo.ViewTypeName;
                    break;

                case "Caption":
                    info.CellData = vwInfo.Caption;
                    break;

                case "IsUsed":
                    info.CellData = vwInfo.IsUsed;
                    break;
            }
        }

        public void VirtualTreeSetCellValue(VirtualTreeSetCellValueInfo info)
        {
            ViewMemberInfo vwInfo = info.Node as ViewMemberInfo;
            switch (info.Column.FieldName)
            {
                case "Caption":
                    vwInfo.Caption = (string)info.NewCellData;
                    break;
            }
        }
        #endregion
    }
    #endregion
}
