using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Globalization;

namespace RapidInterface
{
    // Этот класс позволяет отображать тип GradientParameters в 
    // строку, что позволяет показывать его в диалоге свойств.
    class SecurityConverter : TypeConverter
    {

        // Перекрываем метод CanConvertFrom.
        // Интерфейс ITypeDescriptorContext передает контекст преобразования.
        public override bool CanConvertFrom(ITypeDescriptorContext context,
           Type sourceType)
        {
            // Разрешаем преобразовывать string
            if (sourceType == typeof(string))
            {
                return true;
            }
            // Вызываем базовый метод
            return base.CanConvertFrom(context, sourceType);
        }

        // Перекрываем метод ConvertFrom. Производит преобразование
        // в пользовательский тип.
        public override object ConvertFrom(ITypeDescriptorContext context,
           CultureInfo culture, object value)
        {
            // Преобразовываем строку в GradientParameters
            if (value is string)
            {
                return Security.Parse(value as string);
            }
            return base.ConvertFrom(context, culture, value);
        }

        // Перекрываем метод ConvertTo. Производит преобразование
        // из пользовательского типа.
        // Перекрываем метод ConvertTo. Производит преобразование
        // из пользовательского типа.
        public override object ConvertTo(ITypeDescriptorContext context,
           CultureInfo culture, object value, Type destinationType)
        {
            // Преобразовываем GradientParameters в строку
            if (destinationType == typeof(string))
            {
                if (value != null)
                {
                    if (value is Security)
                    {
                        return ((Security)value).ToString();
                    }
                    else
                    {
                        return value;
                    }
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }


        // Возвращает true, если нужно отображать свойства класса
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        // Возвращает коллекцию свойств, которые нужно отображать
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            //return TypeDescriptor.GetProperties(typeof(Security));
            return TypeDescriptor.GetProperties(typeof(Security)).Sort(new string[] { "User", "Password" });

        }
    }

}
