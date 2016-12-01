using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Usable;

namespace RapidInterface
{
    public class XtraFormOnline : XtraFormEx
    {
        public XtraFormOnline()
        {
            Online = true;

            TriggerPostionX = new TriggerT<int>();
            TriggerPostionY = new TriggerT<int>();

            TriggerOnline = new TriggerT<bool>();
            TriggerOnline.Init(Online);

            KeyPress += XtraFormOnline_KeyPress;
            MouseClick += XtraFormOnline_MouseClick;
            VisibleChanged += XtraFormOnline_VisibleChanged;

            OnlineLimit = OnlineLost = 600;
        }

        /// <summary>
        /// Состояния онлайна.
        /// </summary>
        [DefaultValue("true")]
        [Category("Online")]
        [ReadOnly(true)]
        public bool Online { get; set; }

        /// <summary>
        /// Предел ДК.
        /// </summary>
        [DefaultValue("600")]
        [Category("Online")]
        public int OnlineLimit { get; set; }

        /// <summary>
        /// Событие на изменение онлайна.
        /// </summary>
        [Category("Online")]
        public event EventHandler OnlineChanged;

        /// <summary>
        /// Событие на изменение оставшегося времени онлайна.
        /// </summary>
        [Category("Online")]
        public event EventHandler OnlineCountChanged;

        /// <summary>
        /// Таймер на изменения положения курсора на экране.
        /// </summary>
        ThreadTimer PositionThread;

        /// <summary>
        /// Триггер на изменение X-позиции.
        /// </summary>
        TriggerT<int> TriggerPostionX { get; set; }

        /// <summary>
        /// Триггер на изменение Y-позиции.
        /// </summary>
        TriggerT<int> TriggerPostionY { get; set; }

        /// <summary>
        /// Триггер на изменение Online.
        /// </summary>
        TriggerT<bool> TriggerOnline { get; set; }

        /// <summary>
        /// Оставшиеся циклы до активации закрытия окна.
        /// </summary>
        public int OnlineLost { get; set; }

        delegate void SimpleDel();
        /// <summary>
        /// Делегат на событие OnlineChanged.
        /// </summary>
        SimpleDel OnlineChangedDelegate;

        /// <summary>
        /// Делегат на событие OnlineCountChanged.
        /// </summary>
        SimpleDel OnlineCountChangedDelegate;

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // XtraFormOnline
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "XtraFormOnline";
            this.Load += new System.EventHandler(this.XtraFormOnline_Load);
            this.ResumeLayout(false);
        }


        void OnlineChangedMethod()
        {
            if (OnlineChanged != null)
                OnlineChanged(this, EventArgs.Empty);
        }

        void OnlineCountChangedMethod()
        {
            if (OnlineCountChanged != null)
                OnlineCountChanged(this, EventArgs.Empty);
        }

        private void XtraFormOnline_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Старт мониторинга онлайна.
        /// </summary>
        public void RunOnline()
        {
            PositionThread = new ThreadTimer();
            PositionThread.Period = 1000;
            PositionThread.Delay = 500;
            PositionThread.WorkChanged += PositionThread_WorkChanged;
            PositionThread.InterfaceChanged += PositionThread_InterfaceChanged;
            PositionThread.Run();
            OnlineChangedDelegate = new SimpleDel(OnlineChangedMethod);
            OnlineCountChangedDelegate = new SimpleDel(OnlineCountChangedMethod);
        }

        private void XtraFormOnline_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            OnlineLost = OnlineLimit;
        }

        private void XtraFormOnline_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            OnlineLost = OnlineLimit;
        }

        private void PositionThread_WorkChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                if (TriggerPostionX.Calculate(System.Windows.Forms.Cursor.Position.X) ||
                    TriggerPostionY.Calculate(System.Windows.Forms.Cursor.Position.Y))
                    OnlineLost = OnlineLimit;

                OnlineLost--;

                if (OnlineLost <= 0)
                    OnlineLost = 0;

                if (OnlineLost == 0)
                    Online = false;
                else
                    Online = true;

                try
                {
                    if (TriggerOnline.Calculate(Online))
                        this.Invoke(this.OnlineChangedDelegate);

                    this.Invoke(this.OnlineCountChangedDelegate);
                }
                catch
                { }
            }
        }

        private void XtraFormOnline_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
                OnlineLost = OnlineLimit;
        }

        private void PositionThread_InterfaceChanged(object sender, EventArgs e)
        {
        }
    }
}
