using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace RapidInterface
{
    // Конвертер значений в описания и обратно
    class EnumTypeConverter : EnumConverter
    {
        private Type enumType;

        // Конструктор запоминает типа перечисления
        public EnumTypeConverter(Type enumType)
            : base(enumType)
        {
            this.enumType = enumType;
        }

        // Разрешаем преобразование перечисления в строку
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
        {
            return destType == typeof(string);
        }

        // Конвертируем перечисление в строку
        public override object ConvertTo(ITypeDescriptorContext context,
                                          CultureInfo culture,
                                          object value, Type destType)
        {
            // Получаем значение аттрибута Description
            // для значения перечисления
            FieldInfo fi = enumType.GetField(Enum.GetName(enumType, value));
            DescriptionAttribute da =
               (DescriptionAttribute)Attribute.GetCustomAttribute(
                  fi, typeof(DescriptionAttribute));

            // Если аттрибут не задат, возвращаем 
            // значение как есть
            if (da != null)
                return da.Description;
            else
                return value.ToString();
        }

        // Разрешаем конвертировать перечисление из строки
        public override bool CanConvertFrom(ITypeDescriptorContext context,
                                             Type srcType)
        {
            return srcType == typeof(string);
        }

        // Конвертируем строку в значение перечисления
        public override object ConvertFrom(ITypeDescriptorContext context,
                                            CultureInfo culture,
                                            object value)
        {
            // Надо найти тот элемент перечисления
            // у которого аттрибут Description равен
            // искомому значению
            foreach (FieldInfo fi in enumType.GetFields())
            {
                DescriptionAttribute da =
                   (DescriptionAttribute)Attribute.GetCustomAttribute(
                      fi, typeof(DescriptionAttribute));

                if ((da != null) && ((string)value == da.Description))
                    return Enum.Parse(enumType, fi.Name);
            }
            // Если не нашли значение в аттрибутах
            // пытаемся преобразовать строку в перечисление
            return Enum.Parse(enumType, (string)value);
        }

    }

}
