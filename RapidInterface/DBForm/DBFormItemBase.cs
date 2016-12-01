using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Design;
using DevExpress.Utils.Design;
using DevExpress.Utils;
using DevExpress.XtraNavBar;
using DevExpress.XtraBars.Docking2010.Views;

namespace RapidInterface
{
    #region DBInterfaceItemBase
    /// <summary>
    /// Класс-родитель для элементов быстрого интерфейса.
    /// </summary>
    [DesignTimeVisible(false)]
    public class DBFormItemBase : Component
    {
        public DBFormItemBase()
        {
            Caption = "";
            ImageIndex = -1;
            ImageName = "";
            IsDocumentActivated = false;
        }

        DBForm _DBForm;
        /// <summary>
        /// Обсновной компонент.
        /// </summary>
        [Browsable(false)]
        [Category("Main component")]
        public DBForm DBForm
        {
            get { return _DBForm; }
            set
            {
                if (_DBForm == value) return;
                _DBForm = value;
                Images = value.Icons;
                InvokePropertyChanged();
            }
        }

        Type _ViewType;
        /// <summary>
        /// Тип интерфейса.
        /// </summary>
        [TypeConverter(typeof(XPSourceConverter<DBViewBase>))]
        public Type ViewType
        {
            get { return _ViewType; }
            set
            {
                if (_ViewType == value) return;
                _ViewType = value;
                InvokePropertyChanged();
            }
        }

        /// <summary>
        /// Представление типа поля.
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
                if (BaseNavBarItem != null)
                    return BaseNavBarItem.SmallImageIndex;
                else
                    return -1;
            }
            set
            {
                if (BaseNavBarItem != null)
                    BaseNavBarItem.SmallImageIndex = value;

                InvokePropertyChanged();
            }
        }

        /// <summary>
        /// Название иконки.
        /// </summary>
        [Category("Appearance")]
        public virtual string ImageName { get; set; }

        /// <summary>
        /// Видимость ссылки в панели навигации.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(true)]
        [Category("Appearance")]
        public virtual bool Visible
        {
            get
            {
                if (BaseNavBarItem != null)
                    return BaseNavBarItem.Visible;
                else
                    return false;
            }
            set
            {
                if (BaseNavBarItem != null)
                    BaseNavBarItem.Visible = value;
            }
        }

        /// <summary>
        /// Компонент коллекции иконок.
        /// </summary>
        [Browsable(false)]
        public object Images { get; set; }

        /// <summary>
        /// Владелец-коллекция.
        /// </summary>
        [Category("Invisible components")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public DBFormItemBases Owner { get; set; }

        NavBarItem _BaseNavBarItem;
        /// <summary>
        /// Компонент навигации.
        /// </summary>
        [Category("Form components")]
        public NavBarItem BaseNavBarItem
        {
            get { return _BaseNavBarItem; }
            set
            {
                if (_BaseNavBarItem == value) return;
                _BaseNavBarItem = value;
                InvokePropertyChanged();
            }
        }

        /// <summary>
        /// Форма.
        /// </summary>
        public DBViewBase View { get; set; }

        /// <summary>
        /// Документ, отображающий форму.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BaseDocument Document { get; set; }

        /// <summary>
        /// Активирован документ.
        /// </summary>
        public bool IsDocumentActivated { get; set; }

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
        /// Обновление отображения надписи.
        /// </summary>
        public virtual void UpdateCaption()
        {
            if (DBForm != null && DBForm.isDesignMode())
            {
                if (BaseNavBarItem != null)
                    BaseNavBarItem.Caption = Caption;
            }
        }

        /// <summary>
        /// Обновить все.
        /// </summary>
        public virtual void UpdateAll()
        {
            //UpdateFieldName();
            UpdateCaption();
        }

        /// <summary>
        /// Коррекция индексов иконок.
        /// </summary>
        public void CorrectImageIndex()
        {
            if (ImageName != "" && DBForm != null && DBForm.isDesignMode())
                ImageIndex = ImageEx.GetImageIndex(DBForm.Icons, ImageName);
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

    #region DBInterfaceItemBases
    /// <summary>
    /// Коллекция класса DBInterfaceItemBase.
    /// </summary>
    public class DBFormItemBases : Collection<DBFormItemBase>
    {
        public DBFormItemBases()
        {
        }
        public DBFormItemBases(IList<DBFormItemBase> list)
            : base(list)
        {
            
        }         

        protected override void InsertItem(int index, DBFormItemBase item)
        {
            item.Owner = this;
            base.InsertItem(index, item);
        }

        /// <summary>
        /// Копирование элентов в коллекцию.Ф
        /// </summary>
        public void CopyTo(DBFormItemBases dbRecieves)
        {
            foreach (DBFormItemBase item in this)
                dbRecieves.Add(item);
        }

        /// <summary>
        /// Обмен записями.
        /// </summary>
        public void Change(int first, int second)
        {
            DBFormItemBase dbBuff = this[first];
            this[first] = this[second];
            this[second] = dbBuff;
        }

        /// <summary>
        /// Коррекция индексов иконок.
        /// </summary>
        public void CorrectImageIndex()
        {
            foreach (DBFormItemBase item in this)
                item.CorrectImageIndex();
        }
    }
    #endregion

}
