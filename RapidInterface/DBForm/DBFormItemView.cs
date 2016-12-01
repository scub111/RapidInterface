using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using DevExpress.Xpo;

namespace RapidInterface
{
    #region DBFormItemView
    public class DBFormItemView : DBFormItemBase
    {
        Type _TableType;
        /// <summary>
        /// Тип таблицы.
        /// </summary>
        [TypeConverter(typeof(XPSourceConverter<XPBaseObject>))]
        public Type TableType
        {
            get { return _TableType; }
            set
            {
                if (_TableType == value) return;
                _TableType = value;
                InvokePropertyChanged();
            }
        }
    }
    #endregion
}
