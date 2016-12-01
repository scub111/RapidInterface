using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using DevExpress.Xpo;
using System.ComponentModel;

namespace RapidInterface
{
    #region XPCollectionContainer

    /// <summary>
    /// Коллекция класса DBInterfaceItemBase.
    /// </summary>
    public class XPCollectionContainer : Collection<Component>
    {
        public XPCollectionContainer()
        {
            
        }

        public XPCollectionContainer(IList<Component> list)
            : base(list)
        {
            
        }

        /// <summary>
        /// Поиск XPCollection нужного типа.
        /// </summary>
        public XPCollection IsExistXPCollection(Type type)
        {
            foreach (XPCollection xpCollection in this)
                if (xpCollection.ObjectType == type)
                    return xpCollection;

            return null;
        }

        /// <summary>
        /// Поиск определенного типа во всех привязанных XPCollection.
        /// </summary>
        public static bool IsUsedXPCollection(DBInterfaceItemBases itemsSeq, Type type)
        {
            foreach (DBInterfaceItemBase item in itemsSeq)
            {
                if (item is DBInterfaceItemXPObject)
                {
                    DBInterfaceItemXPObject xpItem = (DBInterfaceItemXPObject)item;
                    if (xpItem.DataSource is XPCollection &&
                        ((XPCollection)xpItem.DataSource).ObjectType == type)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Поиск неиспользуемых объектов коллекции.
        /// </summary>
        public XPCollectionContainer FindUnuseCollections(DBInterfaceItemBases itemsSeq, object collectionExept)
        {
            XPCollectionContainer unuses = new XPCollectionContainer();
            foreach (XPCollection collection in this)
            {
                if (collection != collectionExept)
                {
                    if (collection.ObjectType == null)
                        unuses.Add(collection);

                    if (!IsUsedXPCollection(itemsSeq, collection.ObjectType))
                        unuses.Add(collection);
                }
            }
            return unuses;
        }

        /// <summary>
        /// Перезагрузить данных в источники данных всего списка.
        /// </summary>
        public void Reload()
        {
            foreach (Component collection in this)
                if (collection is XPCollection && ((XPCollection)collection).LoadingEnabled)
                    if (collection is XPCollection && ((XPCollection)collection).LoadingEnabled)
                    ((XPCollection)collection).Reload();
        }

        /// <summary>
        /// Извлечение данных из БД определенной сессии.
        /// </summary>
        public static void GetTable<Type>(Session targetSession)
        {
            if (TypeEx.IsSubclassOf(typeof(Type), typeof(XPObjectEx)))
            {
                XPCollection<Type> stores = new XPCollection<Type>(targetSession);

                foreach (Type store in stores)
                {
                    XPObjectEx newItem = Activator.CreateInstance(typeof(Type)) as XPObjectEx;
                    newItem.GetData<Type>(store);
                    newItem.Save();
                }
            }
        }

        /// <summary>
        /// Сохранение данных в БД определенной сессии.
        /// </summary>
        public static void SaveTable<Type>(Session targetSession)
        {
            if (TypeEx.IsSubclassOf(typeof(Type), typeof(XPObjectEx)))
            {
                XPCollection<Type> stores = new XPCollection<Type>();
                foreach (Type store in stores)
                {
                    XPObjectEx newItem = Activator.CreateInstance(typeof(Type), targetSession) as XPObjectEx;
                    newItem.GetData<Type>(store);
                    newItem.Save();
                }
            }
        }

        /// <summary>
        /// Инициализации коллекции данных.
        /// </summary>
        public static void InitXPCollection(XPCollection collection, Type type, Session session = null)
        {            
            ((System.ComponentModel.ISupportInitialize)(collection)).BeginInit();
            collection.DeleteObjectOnRemove = true;
            collection.ObjectType = type;
            if (session != null)
                collection.Session = session;
            ((System.ComponentModel.ISupportInitialize)(collection)).EndInit();
        }

        /// <summary>
        /// Инициализации серверной коллекции данных.
        /// </summary>
        public static void InitSVCollection(XPServerCollectionSource collection, Type type, Session session)
        {
            ((System.ComponentModel.ISupportInitialize)(collection)).BeginInit();
            collection.DeleteObjectOnRemove = true;
            collection.ObjectType = type;
            if (session != null)
                collection.Session = session;
            ((System.ComponentModel.ISupportInitialize)(collection)).EndInit();
        }
    }
    #endregion
}
