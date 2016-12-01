using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraLayout;
using DevExpress.XtraEditors;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Design;
using DevExpress.Xpo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.Utils.Design;
using DevExpress.Utils;
using System.Reflection;
using DevExpress.XtraGrid.Views.BandedGrid;
using System.Drawing;

namespace RapidInterface
{
    #region DBInterfaceItemBase
    /// <summary>
    /// Класс-родитель для элементов быстрого интерфейса.
    /// </summary>
    [DesignTimeVisible(false)]
    public class DBInterfaceItemBase : Component
    {
        public DBInterfaceItemBase()
        {
            Caption = "";
            FieldName = "";
            Parent = null;
            Owner = null;
            ImageIndex = -1;
            ImageName = "";
        }

        /// <summary>
        /// Родительский объект.
        /// </summary>
        [Category("Invisible components")]
        public DBInterfaceItemBase Parent { get; set; }

        /// <summary>
        /// Владелец-коллекция.
        /// </summary>
        [Category("Invisible components")]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //[Browsable(false)]
        public DBInterfaceItemBases Owner { get; set; }


        DBInterface _DBInterface;
        /// <summary>
        /// Обсновной компонент.
        /// </summary>
        [Browsable(false)]
        [Category("Main component")]
        public DBInterface DBInterface
        {
            get { return _DBInterface; }
            set
            {
                if (_DBInterface == value) return;
                _DBInterface = value;
                Images = value.Icons;
                InvokePropertyChanged();
            }
        }

        string _Caption;
        /// <summary>
        /// Название поля элемента интерфейса.
        /// </summary>
        [Category("Common"), DefaultValue("")]
        public virtual string Caption
        {
            get { return _Caption; }
            set
            {
                if (_Caption == value) return;
                _Caption = value;
                UpdateCaption();
                InvokePropertyChanged();
            }
        }

        string _FieldName;
        /// <summary>
        /// Название поля из базы данных.
        /// </summary>
        [Category("Data"), DefaultValue("")]
        public virtual string FieldName
        {
            get { return _FieldName; }
            set
            {
                if (_FieldName == value) return;
                _FieldName = value;
                UpdateFieldName();
                InvokePropertyChanged();
            }
        }

        /// <summary>
        /// Конечное названия поля из базы данных.
        /// </summary>
        [Category("Data"), DefaultValue("")]
        public virtual string FieldNameEnd
        {
            get
            {
                return FieldName;
            }
        }

        /// <summary>
        /// Название основого компонента на вкладке "Форма".
        /// </summary>
        public string ControlName { get; set; }

        LayoutControlItem _FormLayoutItem;
        /// <summary>
        /// Компонент разметки на вкладце "Форма".
        /// </summary>
        [Category("Form components")]
        public LayoutControlItem FormLayoutItem
        {
            get { return _FormLayoutItem; }
            set
            {
                if (_FormLayoutItem == value) return;
                _FormLayoutItem = value;
                InvokePropertyChanged();
            }
        }

        Component _FormEdit;
        /// <summary>
        /// Компонент редактирования на вкладке "Форма".
        /// </summary>
        [Category("Form components")]
        public virtual Component FormEdit
        {
            get { return _FormEdit; }
            set
            {
                if (_FormEdit == value) return;
                _FormEdit = value;
                InvokePropertyChanged();
            }
        }

        GridColumn _FormGridColumn;
        /// <summary>
        /// Столбец таблицы на вкладке "Таблицы".
        /// </summary>
        [Category("Form components")]
        public GridColumn FormGridColumn
        {
            get { return _FormGridColumn; }
            set
            {
                if (_FormGridColumn == value) return;
                _FormGridColumn = value;
                InvokePropertyChanged();
            }
        }

        GridColumn _TableGridColumn;
        /// <summary>
        /// Столбец таблицы на вкладке "Таблицы".
        /// </summary>
        [Category("Table components")]
        public GridColumn TableGridColumn
        {
            get { return _TableGridColumn; }
            set
            {
                if (_TableGridColumn == value) return;
                _TableGridColumn = value;
                InvokePropertyChanged();
            }
        }

        [Category("Appearance")]
        [Editor(typeof(ImageIndexesEditor), typeof(UITypeEditor))]
        [ImageList("Images")]
        [DefaultValue(-1)]
        /// <summary>
        /// Индекс иконки.
        /// </summary>
        public virtual int ImageIndex 
        {
            get
            {
                if (FormLayoutItem != null)
                    return FormLayoutItem.ImageIndex;
                else if (FormGridColumn != null)
                    return FormGridColumn.ImageIndex;
                else
                    return -1;
            }
            set
            {
                if (FormLayoutItem != null)
                    FormLayoutItem.ImageIndex = value;
                if (FormGridColumn != null)
                    FormGridColumn.ImageIndex = value;
                if (TableGridColumn != null)
                    TableGridColumn.ImageIndex = value;

                if (DBInterface != null && DBInterface.IsDesignModeEx)
                    ImageName = ImageEx.GetImageName(DBInterface.Icons, value);

                InvokePropertyChanged();
            }
        }

        /// <summary>
        /// Название иконки.
        /// </summary>
        [Category("Appearance")]
        public virtual string ImageName { get; set; }

        /// <summary>
        /// Положение колонки в таблице.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Appearance")]
        public virtual int VisibleIndex
        {
            get
            {
                if (FormGridColumn != null)
                    return FormGridColumn.VisibleIndex;
                else if (TableGridColumn != null)
                    return TableGridColumn.VisibleIndex;
                return -1;
            }
            set
            {
                if (FormGridColumn != null)
                    FormGridColumn.VisibleIndex = value;
                if (TableGridColumn != null)
                    TableGridColumn.VisibleIndex = value;
            }
        }

        /// <summary>
        /// Положение колонки в таблице.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Appearance")]
        public virtual bool Visible
        {
            get
            {
                if (FormGridColumn != null)
                    return FormGridColumn.Visible;
                else if (TableGridColumn != null)
                    return TableGridColumn.Visible;
                return false;
            }
            set
            {
                if (FormGridColumn != null)
                    FormGridColumn.Visible = value;
                if (TableGridColumn != null)
                    TableGridColumn.Visible = value;
            }
        }

        /// <summary>
        /// Только для чтения.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Appearance")]
        [DefaultValue (false)]
        public virtual bool ReadOnly
        {
            get
            {
                if (FormEdit != null && FormEdit is BaseEdit)
                    return ((BaseEdit)FormEdit).Properties.ReadOnly;
                else if (FormGridColumn != null)
                    return FormGridColumn.OptionsColumn.ReadOnly;
                else if (TableGridColumn != null)
                    return TableGridColumn.OptionsColumn.ReadOnly;
                return false;
            }
            set
            {
                if (FormGridColumn != null)
                {
                    if (value)
                        FormGridColumn.AppearanceCell.BackColor = DBInterface.ReadOnlyBackColor;
                    else
                        FormGridColumn.AppearanceCell.BackColor = Color.Empty;
                }
                if (TableGridColumn != null)
                {
                    if (value)
                        TableGridColumn.AppearanceCell.BackColor = DBInterface.ReadOnlyBackColor;
                    else
                        TableGridColumn.AppearanceCell.BackColor = Color.Empty;

                    TableGridColumn.OptionsColumn.ReadOnly = value;
                }

                if (FormEdit != null && FormEdit is BaseEdit)
                    ((BaseEdit)FormEdit).Properties.ReadOnly = value;
                if (FormGridColumn != null)                    
                    FormGridColumn.OptionsColumn.ReadOnly = value;
                if (TableGridColumn != null)
                    TableGridColumn.OptionsColumn.ReadOnly = value;
            }
        }

        /// <summary>
        /// Компонент коллекции иконок.
        /// </summary>
        [Browsable(false)]
        public object Images { get; set; }

        /// <summary>
        /// Получение имени типа в зависимости от его типа.
        /// </summary>
        public virtual string TypeName
        {
            get
            {
                return "Base";
            }
        }

        /// <summary>
        /// Событие на изменение какого-нибудь свойства.
        /// </summary>
        public event EventHandler PropertyChanged;

        /// <summary>
        /// Метод оповещения события PropertyChanged.
        /// </summary>
        public void InvokePropertyChanged()
        {
            if (PropertyChanged != null)
                PropertyChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Обновление привязки данных.
        /// </summary>
        public virtual void UpdateFieldName()
        {
            if (DBInterface != null && DBInterface.IsDesignModeEx)
            {
                if (DBInterface.BaseXPCollecton != null && FormEdit != null && FormEdit is BaseEdit)
                {
                    BaseEdit baseEdit = FormEdit as BaseEdit;
                    if (baseEdit.DataBindings.Count > 0)
                        baseEdit.DataBindings.Clear();
                    if (DBInterface.BaseXPCollecton != null)
                        baseEdit.DataBindings.Add(new Binding("EditValue", DBInterface.BaseXPCollecton, FieldNameEnd, true));
                }
                if (FormGridColumn != null &&
                    FormGridColumn.FieldName != FieldName)
                    FormGridColumn.FieldName = FieldNameEnd;

                if (TableGridColumn != null &&
                    TableGridColumn.FieldName != FieldName)
                    TableGridColumn.FieldName = FieldNameEnd;
            }
        }

        /// <summary>
        /// Обновление отображения надписи.
        /// </summary>
        public virtual void UpdateCaption()
        {
            if (DBInterface != null && DBInterface.IsDesignModeEx)
            {
                if (FormLayoutItem != null && FormLayoutItem.Text != Caption + ":")
                {
                    FormLayoutItem.CustomizationFormText = Caption;
                    FormLayoutItem.Text = Caption + ":";
                }

                if (FormGridColumn != null &&
                    FormGridColumn.Caption != Caption)
                    FormGridColumn.Caption = Caption;

                if (TableGridColumn != null &&
                    TableGridColumn.Caption != Caption)
                    TableGridColumn.Caption = Caption;
            }
        }

        /// <summary>
        /// Коррекция индексов иконок.
        /// </summary>
        public void CorrectImageIndex()
        {
            if (ImageName != "" && DBInterface != null && DBInterface.IsDesignModeEx)
                ImageIndex = ImageEx.GetImageIndex(DBInterface.Icons, ImageName);
        }

        /// <summary>
        /// Задание имени компоненту взависимости от его свойства FieldName.
        /// </summary>
        public string GetComponentNameByFieldName()
        {
            string name = "Unknown";
            if (FieldName != "")
                name = FieldName;
            return name;
        }

        /// <summary>
        /// Обновить все.
        /// </summary>
        public virtual void UpdateAll()
        {
            UpdateFieldName();
            UpdateCaption();
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
        private int GetLevel(DBInterfaceItemBase dbItem, int level)
        {
            if (dbItem.Parent != null)
                return GetLevel(dbItem.Parent, level + 1);
            else
                return level;
        }

        /// <summary>
        /// Метод определения наивысшего родителя записи.
        /// </summary>
        public DBInterfaceItemBase GetTopParent()
        {
            return GetTopParent(this, 1);
        }

        /// <summary>
        /// Рекурсивный метод определения наивысшего родителя записи.
        /// </summary>
        private DBInterfaceItemBase GetTopParent(DBInterfaceItemBase dbItem, int level)
        {
            if (dbItem.Parent != null)
                return GetTopParent(dbItem.Parent, level + 1);
            else
                return dbItem;
        }

        /// <summary>
        /// Вовзврат индекс в коллекции, где располагается объект.
        /// </summary>
        /// <returns></returns>
        public int GetIndex()
        {
            return Owner.IndexOf(this);
        }

        /// <summary>
        /// Если элемент первый по счету.
        /// </summary>
        public bool IsFirst()
        {
            if (GetIndex() == 0)
                return true;
            return false;
        }

        /// <summary>
        /// Если элемент последний по счету.
        /// </summary>
        public bool IsLast()
        {
            if (GetIndex() == Owner.Count - 1)
                return true;
            return false;
        }
    }
    #endregion

    #region DBInterfaceItemBoolean
    /// <summary>
    /// Класс для отображения элементов булевой логики.
    /// </summary>
    public class DBInterfaceItemBoolean : DBInterfaceItemBase
    {
        public DBInterfaceItemBoolean()
        {
        }

        public override string TypeName
        {
            get
            {
                return "Boolean";
            }
        }

        public override void UpdateCaption()
        {
            base.UpdateCaption();
            if (DBInterface != null && DBInterface.IsDesignModeEx)
            {
                if (FormEdit != null && FormEdit is Control && ((Control)FormEdit).Text != Caption)
                    ((Control)FormEdit).Text = Caption;
            }
        }
    }
    #endregion

    #region DBInterfaceItemString
    /// <summary>
    /// Класс для отображения стороковых элементов.
    /// </summary>
    public class DBInterfaceItemString : DBInterfaceItemBase
    {
        public DBInterfaceItemString()
        {
        }

        public override string TypeName
        {
            get
            {
                return "String";
            }
        }
    }
    #endregion

    #region DBInterfaceItemDateTime
    /// <summary>
    /// Класс для отображения элементов времени.
    /// </summary>
    public class DBInterfaceItemDateTime : DBInterfaceItemBase
    {
        public DBInterfaceItemDateTime()
        {
        }

        /// <summary>
        /// Компонент для редактирования времени в таблице.
        /// </summary>
        [Category("Visible components")]
        public RepositoryItemDateEditEx RepositoryItemDateEdit { get; set; }
        
        /// <summary>
        /// Формат строки.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Appearance")]
        [DefaultValue("d")]
        public string FormatString
        {
            get
            {
                if (FormEdit != null && FormEdit is DateEditEx)
                    return ((DateEditEx)FormEdit).Properties.DisplayFormat.FormatString;
                else if (RepositoryItemDateEdit != null)
                    return RepositoryItemDateEdit.DisplayFormat.FormatString;
                else
                    return "";
            }
            set
            {
                if (FormEdit != null && FormEdit is DateEditEx)
                {
                    ((DateEditEx)FormEdit).Properties.DisplayFormat.FormatString = value;
                    ((DateEditEx)FormEdit).Properties.EditFormat.FormatString = value;
                    ((DateEditEx)FormEdit).Properties.Mask.EditMask = value;
                }

                if (RepositoryItemDateEdit != null)
                {
                    RepositoryItemDateEdit.DisplayFormat.FormatString = value;
                    RepositoryItemDateEdit.EditFormat.FormatString = value;
                    RepositoryItemDateEdit.Mask.EditMask = value;
                }
            }
        }

        public override string TypeName
        {
            get
            {
                return "DateTime";
            }
        }
    }
    #endregion

    #region DBInterfaceItemNumeric
    /// <summary>
    /// Класс для отображения элементов чисел.
    /// </summary>
    public class DBInterfaceItemNumeric : DBInterfaceItemBase
    {
        public DBInterfaceItemNumeric()
        {
        }

        public override string TypeName
        {
            get
            {
                return "Numeric";
            }
        }
    }
    #endregion

    #region DBInterfaceItemXPComplex
    /// <summary>
    /// Класс для отображения элементов булевой логики.
    /// </summary>
    public class DBInterfaceItemXPComplex : DBInterfaceItemBase
    {
        public DBInterfaceItemXPComplex()
        {
            _Items = new DBInterfaceItemBases(this, true);
        }

        DBInterfaceItemBases _Items;
        /// <summary>
        /// Описание полей, которые будут отображаться в таблице.
        /// </summary>
        [Category("Invisible components")]
        [Editor(typeof(DBInterfaceItemCollectionEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DBInterfaceItemBases Items
        {
            get { return _Items; }
        }

        /// <summary>
        /// Вид основого объекта таблицы.
        /// </summary>
        [Category("Form components")]
        public GridView FormGridView { get; set; }

        /// <summary>
        /// Вид выпадающего объекта таблицы.
        /// </summary>
        [Category("Table components")]
        public GridView TableGridView { get; set; }
    }
    #endregion

    #region XPObjectEventArgs
    /// <summary>
    /// Класс для события при нажатии правой кнопки мыши по выпадающему списку.
    /// </summary>
    public class XPObjectEventArgs : EventArgs
    {
        /// <summary>
        /// Тип таблицы.
        /// </summary>
        public Type TableType { get; set; }

        /// <summary>
        /// Запись.
        /// </summary>
        public XPBaseObject Record { get; set; }
    }
    #endregion

    #region DBInterfaceItemXPObject
    public class DBInterfaceItemXPObject : DBInterfaceItemXPComplex
    {
        public DBInterfaceItemXPObject()
        {
        }

        public override Component FormEdit
        {
            get
            {
                return base.FormEdit; 
            }
            set
            {
                base.FormEdit = value;
                if (FormEdit != null)
                {
                    if (FormEdit is GridLookUpEdit)
                        ((GridLookUpEdit)FormEdit).MouseDown += new MouseEventHandler(ControlEdit_MouseDown);
                    if (FormEdit is RepositoryItemGridLookUpEdit)
                        ((RepositoryItemGridLookUpEdit)FormEdit).MouseDown += new MouseEventHandler(ControlEdit_MouseDown);
                }
            }
        }

        public override string FieldNameEnd
        {
            get
            {
                return FieldName + "!";
            }
        }

        public override string TypeName
        {
            get
            {
                if (TableType != null)
                    return TableType.Name;
                else
                    return "XPObject [null]";
            }
        }

        /// <summary>
        /// Тип таблицы.
        /// </summary>
        [Category("Invisible components")]
        public Type TableType { get; set; }

        [Category("Data")]
        [DefaultValue("")]
        [Description("Gets or sets the source of data displayed in the dropdown window.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [AttributeProvider(typeof(IListSource))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object DataSource
        {
            get
            {
                if (FormEdit != null)
                    if (FormEdit is GridLookUpEdit)
                        return ((GridLookUpEdit)FormEdit).Properties.DataSource;
                    else if (FormEdit is RepositoryItemGridLookUpEdit)
                        return ((RepositoryItemGridLookUpEdit)FormEdit).DataSource;
                    else
                        return null;
                else
                    return null;
            }
            set
            {
                if (FormEdit != null)
                    if ((FormEdit is GridLookUpEdit))
                        ((GridLookUpEdit)FormEdit).Properties.DataSource = value;
                    else if (FormEdit is RepositoryItemGridLookUpEdit)
                        ((RepositoryItemGridLookUpEdit)FormEdit).DataSource = value;

                if (TableRepositoryEdit != null)
                    TableRepositoryEdit.DataSource = value;
            }
        }

        [Category("Data")]
        [DefaultValue("")]
        [Description("Gets or sets the field whose values are displayed in the edit box.")]
        [TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design")]
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design", typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DisplayMember
        {
            get
            {
                if (FormEdit != null)
                {
                    if (FormEdit is GridLookUpEdit)
                        return ((GridLookUpEdit)FormEdit).Properties.DisplayMember;
                    else if (FormEdit is RepositoryItemGridLookUpEdit)
                        return ((RepositoryItemGridLookUpEdit)FormEdit).DisplayMember;
                    else
                        return null;
                }
                else
                    return null;
            }
            set
            {
                if (FormEdit != null)
                    if (FormEdit is GridLookUpEdit)
                        ((GridLookUpEdit)FormEdit).Properties.DisplayMember = value;
                    else if (FormEdit is RepositoryItemGridLookUpEdit)
                        ((RepositoryItemGridLookUpEdit)FormEdit).DisplayMember = value;

                if (TableRepositoryEdit != null)
                    TableRepositoryEdit.DisplayMember = value;
            }
        }

        [Category("Data")]
        [DefaultValue("")]
        [Description("Gets or sets the field name whose values identify dropdown rows.")]
        [TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design")]
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design", typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ValueMember
        {
            get
            {
                if (FormEdit != null)
                {
                    if (FormEdit is GridLookUpEdit)
                        return ((GridLookUpEdit)FormEdit).Properties.ValueMember;
                    else if (FormEdit is RepositoryItemGridLookUpEdit)
                        return ((RepositoryItemGridLookUpEdit)FormEdit).ValueMember;
                    else
                        return null;
                }
                else
                    return null;
            }
            set
            {
                if (FormEdit != null)
                    if (FormEdit is GridLookUpEdit)
                        ((GridLookUpEdit)FormEdit).Properties.ValueMember = value;
                    else if (FormEdit is RepositoryItemGridLookUpEdit)
                        ((RepositoryItemGridLookUpEdit)FormEdit).ValueMember = value;

                if (TableRepositoryEdit != null)
                    TableRepositoryEdit.ValueMember = value;                    
            }
        }

        /// <summary>
        /// Название вида таблицы.
        /// </summary>
        public string GridViewCaption { get; set; }

        RepositoryItemGridLookUpEdit _TableRepositoryEdit;
        /// <summary>
        /// Вид объекта таблицы для представления "Таблица".
        /// </summary>
        [Category("Table components")]
        public RepositoryItemGridLookUpEdit TableRepositoryEdit
        {
            get { return _TableRepositoryEdit; }
            set
            {
                _TableRepositoryEdit = value;
                if (_TableRepositoryEdit != null)
                    _TableRepositoryEdit.MouseDown += new MouseEventHandler(ControlEdit_MouseDown);
            }
        }

        public delegate void RightMouseDownEventHandler(object sender, XPObjectEventArgs e);
        /// <summary>
        /// Событие для нажатия праковой кнопки мыши по элементу.
        /// </summary>
        public event RightMouseDownEventHandler RightMouseDown;

        void ControlEdit_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                GridLookUpEdit edit = sender as GridLookUpEdit;
                if (edit != null && edit.EditValue is XPBaseObject)
                {
                    XPObjectEventArgs arg = new XPObjectEventArgs();
                    arg.TableType = TableType;
                    arg.Record = edit.EditValue as XPBaseObject;

                    if (RightMouseDown != null)
                        RightMouseDown(sender, arg);

                    if (DBInterface != null)
                        DBInterface.InvokeRightMouseDown(sender, arg);
                }
            }
        }
    }
    #endregion

    #region DBInterfaceItemXPCollection
    /// <summary>
    /// Класс для отображения элементов булевой логики.
    /// </summary>
    public class DBInterfaceItemXPCollection : DBInterfaceItemXPComplex
    {
        public DBInterfaceItemXPCollection()
        {
        }

        public override string TypeName
        {
            get
            {
                string fieldName = GridViewCaption;
                if (FieldNameEnd != "")
                    fieldName = FieldNameEnd;
                return string.Format("XPC<{0}>", fieldName);
            }
        }

        public override int ImageIndex
        {
            get
            {
                if (FormLayoutGroup != null)
                    return FormLayoutGroup.CaptionImageIndex;
                else
                    return -1;
            }
            set
            {
                if (FormLayoutGroup != null)
                    FormLayoutGroup.CaptionImageIndex = value;

                InvokePropertyChanged();
            }
        }

        [Category("Data")]
        [DefaultValue("")]
        [Description("Gets or sets the source of data displayed in the dropdown window.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [AttributeProvider(typeof(IListSource))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object DataSource
        {
            get
            {
                if (FormEdit != null)
                    return ((GridControlEx)FormEdit).DataSource;
                else
                    return null;
            }
            set
            {
                if (FormEdit != null)
                    ((GridControlEx)FormEdit).DataSource = value;
            }
        }

        /// <summary>
        /// Название коллекции.
        /// </summary>
        [DefaultValue("")]
        [Localizable(true)]
        [Category("Data")]
        [Description("Gets or sets a data source member whose data is supplied for the grid control's main View.")]
        [Editor("System.Windows.Forms.Design.DataMemberListEditor, System.Design", typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DataMember
        {
            get
            {
                if (FormEdit != null)
                    return ((GridControlEx)FormEdit).DataMember;
                else
                    return null;
            }
            set
            {
                if (FormEdit != null)
                    ((GridControlEx)FormEdit).DataMember = value;

                if (FormLevelNode != null)
                    FormLevelNode.RelationName = value;                
                
                if (TableLevelNode != null)
                    TableLevelNode.RelationName = value;

                InvokePropertyChanged();
            }
        }

        /// <summary>
        /// Уровень вложения в таблицу во вкладе "Форма".
        /// </summary>
        [Category("Form components")]
        public GridLevelNode FormLevelNode { get; set; }

        /// <summary>
        /// Уровень вложения в таблицу во вкладе "Таблица".
        /// </summary>
        [Category("Table components")]
        public GridLevelNode TableLevelNode { get; set; }

        /// <summary>
        /// Название вида таблицы.
        /// </summary>
        public string GridViewCaption { get; set; }

        /// <summary>
        /// Группа разметки формы.
        /// </summary>
        [Category("Form components")]
        public LayoutControlGroup FormLayoutGroup { get; set; }

        public override void UpdateCaption()
        {
            base.UpdateCaption();
            if (DBInterface != null &&  
                DBInterface.IsDesignModeEx &&
                FormLayoutGroup != null)
            {
                FormLayoutGroup.CustomizationFormText = Caption;
                FormLayoutGroup.Text = Caption;
            }            
        }
    }
    #endregion

    #region DBInterfaceItemBases
    /// <summary>
    /// Коллекция класса DBInterfaceItemBase.
    /// </summary>
    public class DBInterfaceItemBases : Collection<DBInterfaceItemBase>
    {
        public DBInterfaceItemBases(DBInterfaceItemBase parent = null, bool rewrite = false)
        {
            Parent = parent;
            Rewrite = rewrite;
        }

        /// <summary>
        /// Родительский объект.
        /// </summary>
        public DBInterfaceItemBase Parent { get; set; }

        /// <summary>
        /// Перезаписываемый владелец элемента коллекции.
        /// </summary>
        public bool Rewrite { get; set; }

        protected override void InsertItem(int index, DBInterfaceItemBase item)
        {
            base.InsertItem(index, item);
            if (Rewrite)
            {
                item.Owner = this;
                item.Parent = Parent;
            }
        }

        /// <summary>
        /// Коррекция индексов иконок.
        /// </summary>
        public void CorrectImageIndex()
        {
            foreach (DBInterfaceItemBase item in this)
                item.CorrectImageIndex();
        }

        /// <summary>
        /// Установка связей с данными.
        /// </summary>
        public void SetDataBinding(Component collection)
        {
            foreach (DBInterfaceItemBase item in this)
                if (!(item is DBInterfaceItemXPCollection) && item.FormEdit is Control)
                    ((Control)item.FormEdit).DataBindings.Add(new System.Windows.Forms.Binding("EditValue", collection, item.FieldName, true));
        }

        /// <summary>
        /// Коррекция положения колонки в таблице.
        /// </summary>
        public void CorrectVisibleIndex()
        {
            GridView tableGridView = null;
            GridView formGridView = null;

            if (Parent == null)
            {
                tableGridView = this[0].DBInterface.TableGridView;
            }
            else
            {
                if (Parent is DBInterfaceItemXPComplex)
                {
                    tableGridView = ((DBInterfaceItemXPComplex)Parent).TableGridView;
                    formGridView = ((DBInterfaceItemXPComplex)Parent).FormGridView;
                }
            }
            if (tableGridView != null)
                tableGridView.Columns.Clear();

            if (formGridView != null)
                formGridView.Columns.Clear();

            for (int i = 0; i < Count; i++)
                this[i].VisibleIndex = i;
            
            if (formGridView != null)
                for (int i = 0; i < Count; i++)
                    formGridView.Columns.Add(this[i].FormGridColumn);

            if (tableGridView != null)
                for (int i = 0; i < Count; i++)
                    tableGridView.Columns.Add(this[i].TableGridColumn);
        }

        /// <summary>
        /// Копирование элентов в коллекцию.Ф
        /// </summary>
        public void CopyTo(DBInterfaceItemBases dbRecieves)
        {
            foreach (DBInterfaceItemBase item in this)
                dbRecieves.Add(item);
        }

        /// <summary>
        /// Обмен записями.
        /// </summary>
        public void Change(int first, int second)
        {
            DBInterfaceItemBase dbBuff = this[first];
            this[first] = this[second];
            this[second] = dbBuff;
        }
    }
    #endregion
}
