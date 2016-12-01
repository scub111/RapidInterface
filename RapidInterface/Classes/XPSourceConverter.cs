using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace RapidInterface
{
    public class XPSourceConverter<T> : TypeConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true; // display drop
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true; // drop-down vs combo
        }

        /// <summary>
        /// Создание начальных записей в выпадающем списке
        /// </summary>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<Type> result = new List<Type>();

            ITypeDiscoveryService discoveryService = (ITypeDiscoveryService)context.GetService(typeof(ITypeDiscoveryService));
            if (discoveryService == null)
                discoveryService = (ITypeDiscoveryService)((IServiceProvider)((Component)context.Instance).Site).GetService(typeof(ITypeDiscoveryService));

            if (discoveryService != null)
                foreach (Type actionType in discoveryService.GetTypes(typeof(T), false))
                    if (!result.Contains(actionType) && actionType != typeof(T))
                        result.Add(actionType);

            return new StandardValuesCollection(result.ToArray());
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            else
                return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Конвертирование из строки в тип.
        /// </summary>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            ITypeDiscoveryService discoveryService = (ITypeDiscoveryService)context.GetService(typeof(ITypeDiscoveryService));
            if (discoveryService == null)
                discoveryService = (ITypeDiscoveryService)((IServiceProvider)((Component)context.Instance).Site).GetService(typeof(ITypeDiscoveryService));

            if (discoveryService != null)
                foreach (Type actionType in discoveryService.GetTypes(typeof(T), false))
                    if (actionType.FullName == (string)value)
                        return actionType;

            return base.ConvertFrom(context, culture, value);
        }
    }
}
