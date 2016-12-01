using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DevExpress.Utils;
using System.Drawing.Imaging;

namespace RapidInterface
{
    public class ImageEx
    {

        /// <summary>
        /// Существует ли эта иконка в коллекции.
        /// </summary>
        public static bool IsExist(ImageCollection images, string file)
        {
            Image image = images.Images[file];
            if (image != null)
                return true;
            return false;
        }

        /// <summary>
        /// Получение порядкового номера в коллекции иконок.
        /// </summary>
        public static int GetImageIndex(ImageCollection images, string file)
        {
            Image image = images.Images[file];
            if (image != null)
                return images.Images.IndexOf(image);
            return -1;
        }

        public static string GetImageName(ImageCollection images, int index)
        {
            if (index != -1)
                return images.Images.Keys[index];

            return "";
        }
    }
}
