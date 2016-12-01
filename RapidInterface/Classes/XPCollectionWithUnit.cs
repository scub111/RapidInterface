using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using System.Collections.ObjectModel;

namespace RapidInterface
{
    #region XPCollectionWithUnit
    /// <summary>
    /// Класс, объединяющий коллекцию данных и объект связи с БД.
    /// </summary>
    public class XPCollectionWithUnit
    {
        public XPCollectionWithUnit(Type type)
        {
            Collection = new XPCollection();
            UnitOfWork = new UnitOfWork();
            XPCollectionContainer.InitXPCollection(Collection, type, UnitOfWork);
        }

        /// <summary>
        /// Коллекция данных.
        /// </summary>
        public XPCollection Collection { get; set; }

        /// <summary>
        /// Объект для подключения к БД.
        /// </summary>
        public UnitOfWork UnitOfWork { get; set; }
    }
    #endregion

    #region XPCollectionWithUnits
    /// <summary>
    /// Коллекция класса XPCollectionWithUnit.
    /// </summary>
    public class XPCollectionWithUnits : Collection<XPCollectionWithUnit>
    {
        /// <summary>
        /// Дабавление элемента.
        /// </summary>
        public XPCollectionWithUnit Add(Type type)
        {
            XPCollectionWithUnit newCollection = new XPCollectionWithUnit(type);
            Add(newCollection);
            return newCollection;
        }
    }
    #endregion
}
