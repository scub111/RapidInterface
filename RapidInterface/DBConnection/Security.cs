using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace RapidInterface
{
    [TypeConverter(typeof(SecurityConverter))]
    public class Security
    {
        public Security(string user, string password)
        {
            User = user;
            Password = password;
        }

        /// <summary>
        /// Пользователь.
        /// </summary>
        [Category("Security")]
        public string User { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        [Category("Security")]
        public string Password { get; set; }

        // Преобразование строкового представления параметров
        public static Security Parse(string value)
        {
            // Делим строку на две части
            string[] array = ((string)value).Split(new char[] { ',', ' '}, StringSplitOptions.RemoveEmptyEntries);
            return new Security(array[0], array[1]);
        }

        // Преобразование параметров в строку
        public override string ToString()
        {
            return string.Format("{0}, {1}", User, Password);
        }
    }
}
