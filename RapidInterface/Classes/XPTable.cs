using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Collections;

namespace RapidInterface.Classes
{
    class XPTable
    {
        public XPTable(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// Тип.
        /// </summary>
        public Type Type { get; set; }


        /// <summary>
        /// Название типа.
        /// </summary>
        public string TypeFullName 
        {
            get
            {
                return Type.Name; ;
            }
        }
    }

    class XPTables : Collection<XPTable>
    {
        /// <summary>
        /// Поиск нужной записи.
        /// </summary>
        public XPTable Find(Type type)
        {
            foreach (XPTable table in this)
                if (table.Type == type)
                    return table;
            return null;
        }

        /// <summary>
        /// Создание списка возможных типов таблицы.
        /// </summary>
        public void FillTable(ITypeDiscoveryService TypeDiscoveryService, Type type)
        {
            if (TypeDiscoveryService != null)
            {
                ICollection types = TypeDiscoveryService.GetTypes(type, false);
                foreach (Type actionType in types)
                    if (actionType != type)
                        Add(new XPTable(actionType));
            }
        }
    }

}
