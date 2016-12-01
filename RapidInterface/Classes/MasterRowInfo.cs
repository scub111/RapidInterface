using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidInterface
{
    /// <summary>
    /// Класс для хранения информации о раскрытых строках таблицы.
    /// </summary>
    public class MasterRowInfo
    {
        public MasterRowInfo(int rowHandle, int relationIndex)
        {
            RowHandle = rowHandle;
            RelationIndex = relationIndex;
        }

        /// <summary>
        /// Дескриптор строки.
        /// </summary>
        public int RowHandle { get; set; }

        /// <summary>
        /// Индекс видимой детальной таблицы.
        /// </summary>
        public int RelationIndex { get; set; }
    }

}
