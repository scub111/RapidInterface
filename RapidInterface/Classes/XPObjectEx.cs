using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpo;
using System.Collections.ObjectModel;

namespace RapidInterface
{
    #region XPObjectEx
    /// <summary>
    /// Измененный класс XPObject
    /// с немедленным удалением записей.
    /// </summary>
    [OptimisticLocking(false)]
    [DeferredDeletion(false)]
    public class XPObjectEx : XPObject
    {
        public XPObjectEx() : base(Session.DefaultSession)
        {
            Init();
        }

        public XPObjectEx(Session session) : base(session)
        {
            Init();
        }

        protected override void OnSaved()
        {
            base.OnSaved();
        }

        /// <summary>
        /// Инициализация данных.
        /// </summary>
        public virtual void Init()
        {
            AutoSaveOnEndEdit = false;
            if (!IsLoading)
            {
                UserCreatedDate = DateTime.Now;
                UserChangedDate = UserCreatedDate;
                HashCode = GetHashCode();
            }
        }

        /// <summary>
        /// Текст для показа в выпадающем списке.
        /// </summary>
        [NonPersistent]
        public virtual string DisplayMember
        {
            get
            {
                return "";
            }
        }

        /// <summary>
        /// Получение значений с объекта-источника.
        /// </summary>
        public virtual void GetData<Type>(Type source) 
        {
            if (TypeEx.IsSubclassOf(typeof(Type), typeof(XPObjectEx)))
                Oid = (source as XPObjectEx).Oid; 
        }

        /// <summary>
        /// Перерасчет всех контрольных значений.
        /// </summary>
        public virtual void CalculateAll() { }

        DateTime _UserCreatedDate;
        /// <summary>
        /// Время создания записи.
        /// </summary>
        [DisplayName("Время создания")]
        public DateTime UserCreatedDate
        {
            get { return _UserCreatedDate; }
            set { SetPropertyValue("UserCreatedDate", ref _UserCreatedDate, value); }
        }

        DateTime _UserChangedDate;
        /// <summary>
        /// Время изменения записи.
        /// </summary>
        [DisplayName("Время изменения")]
        public DateTime UserChangedDate
        {
            get { return _UserChangedDate; }
            set { SetPropertyValue("UserChangedDate", ref _UserChangedDate, value); }
        }

        int _HashCode;
        /// <summary>
        /// Хеш-код.
        /// </summary>
        [DisplayName("Хеш-код")]
        public int HashCode
        {
            get { return _HashCode; }
            set { SetPropertyValueEx("HashCode", ref _HashCode, value); }
        }

        public bool SetPropertyValueEx<T>(string propertyName, ref T propertyValueHolder, T newValue)
        {
            if (!IsLoading)
                UserChangedDate = DateTime.Now;
            return SetPropertyValue(propertyName, ref propertyValueHolder, newValue);
        }

        public bool SetPropertyValueEx<T>(string propertyName, T newValue)
        {
            if (!IsLoading)
                UserChangedDate = DateTime.Now;
            return SetPropertyValue(propertyName, newValue);
        }

        protected override void OnDeleting()
        {
            base.OnDeleting();
        }  
        
                /// <summary>
        /// Поиск записи из коллекции.
        /// </summary>
        public static int FindRecordPossition(System.ComponentModel.Component сollection, XPBaseObject record)
        {
            if (record == null)
                return 0;

            if (сollection is XPCollection)
            {
                XPCollection xpCollection = сollection as XPCollection;
                string propertyName = DBAttribute.GetKey(record.GetType());
                int idRecord = (int)record.GetMemberValue(propertyName);

                for (int i = 0; i < xpCollection.Count; i++)
                {
                    XPBaseObject recordInt = (XPBaseObject)xpCollection[i];

                    int idRecordInt = (int)recordInt.GetMemberValue(propertyName);

                    if (idRecordInt == idRecord)
                        return i;
                }
            }
            return 0;
        }      
    }
    #endregion

    #region LinkXPObject
    public class LinkXPObject
    {
        public LinkXPObject(XPObject xpObject)
        {
            XPObject = xpObject;
            Oid = xpObject.Oid;
        }

        public object XPObject { get; set; }
        public int Oid { get; set; }

        /// <summary>
        /// Поиск объекта сервера.
        /// </summary>
        public object FindXPOject(Collection<XPObject> collection)
        {
            foreach (XPObject record in collection)
                if (record.Oid == Oid)
                {
                    XPObject = record;
                    return XPObject;
                }
            return null;
        }

        /// <summary>
        /// Отправка данных.
        /// </summary>
        public virtual void SendDataToXPObject() { }

        public static void Transfer<TypeReal>(XPCollection xpCollection, Collection<TypeReal> collectionReal)
        {
            if (TypeEx.IsSubclassOf(typeof(TypeReal), typeof(LinkXPObject)))
            {
                Collection<XPObject> collection = new Collection<XPObject>();
                foreach (XPObject server in xpCollection)
                    collection.Add(server);

                foreach (TypeReal serverReal in collectionReal)
                {
                    LinkXPObject link = serverReal as LinkXPObject;
                    XPObject server = link.FindXPOject(collection) as XPObject;
                    if (server != null)
                        collection.Remove(server);
                }
            }
        }
    }
    #endregion

    #region LinkXPObjects
    public class LinkXPObjects : Collection<LinkXPObject>
    {
        public void SendData()
        {
            foreach (LinkXPObject item in this)
                item.SendDataToXPObject();
        }
    }
    #endregion


}
