using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidInterface
{
    /// <summary>
    /// Класс, передачи резальтатов при проверке на возможность перехода к следующей форме.
    /// </summary>
    public class AllowSwitchMessage
    {
        public AllowSwitchMessage(bool isAllow, bool isMessage)
        {
            IsAllow = isAllow;
            IsMessage = isMessage;
        }

        /// <summary>
        /// Разрешение перехода на слудующую форму.
        /// </summary>
        public bool IsAllow { get; set; }

        /// <summary>
        /// Наличие сообщения при проверке.
        /// </summary>
        public bool IsMessage { get; set; }
    }

}
