using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DevExpress.Xpo;

namespace RapidInterface
{
    #region DbAttribute
    //[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
    public class DBAttribute : Attribute
    {
        public DBAttribute() { ;}

        /// <summary>
        /// Подпись.
        /// </summary>
        public bool DisplayMember { get; set; }

        /// <summary>
        /// Путь к инонке.
        /// </summary>
        public string IconFile { get; set; }

        /// <summary>
        /// Название таблицы
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Получение надписи аттрибута DisplayMember.
        /// </summary>
        public static string GetDisplayMember(Type type)
        {
            PropertyInfo[] infos = type.GetProperties();

            foreach (PropertyInfo info in infos)
            {
                DBAttribute[] dbAttributs =
                    info.GetCustomAttributes(typeof(DBAttribute), false) as DBAttribute[];
                for (int i = 0; i < dbAttributs.Length; i++)
                    if (dbAttributs[i].DisplayMember)
                        return info.Name;
            }
            return infos[0].Name;
        }

        public static string GetKey(Type type)
        {
            if (type == null) return "";

            PropertyInfo[] infos = type.GetProperties();

            foreach (PropertyInfo info in infos)
            {
                KeyAttribute[] keyAttributes =
                    info.GetCustomAttributes(typeof(KeyAttribute), false) as KeyAttribute[];
                if (keyAttributes.Length > 0)
                    return info.Name;
            }
            return infos[0].Name;
        }

        /// <summary>
        /// Получение строки иконки.
        /// </summary>
        public static string GetIconFile(Type type)
        {
            if (type == null) return "";

            DBAttribute[] dbAttributs = type.GetCustomAttributes(typeof(DBAttribute), false) as DBAttribute[];
            if (dbAttributs.Length > 0)
                return dbAttributs[0].IconFile;

            return "";
        }

        /// <summary>
        /// Получение строки названия таблицы.
        /// </summary>
        public static string GetCaption(Type type)
        {
            if (type == null) return "";

            DBAttribute[] dbAttributs = type.GetCustomAttributes(typeof(DBAttribute), false) as DBAttribute[];
            if (dbAttributs.Length > 0)
                return dbAttributs[0].Caption;

            return "";
        }

    }
    #endregion
}
