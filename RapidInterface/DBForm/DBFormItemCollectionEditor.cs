using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RapidInterface
{
    class DBFormItemCollectionEditor : CollectionEditor
    {
        /// <summary>
        /// Список подерживаемых типов.
        /// </summary>
        private Type[] returnedTypes;

        public DBFormItemCollectionEditor(Type type)
            : base(type)
        {
        }

        /// <summary>
        /// Поиск базового компонента.
        /// </summary>
        /// <returns></returns>
        public DBForm FindDBForm()
        {
            if (Context.Instance is DBForm)
                return (DBForm)Context.Instance;
            else if (Context.Instance is DBFormActionList)
                return ((DBFormActionList)Context.Instance).DBForm;
            else
                return null;
        }

        protected override Type[] CreateNewItemTypes()
        {
            return returnedTypes;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (returnedTypes == null)
            {
                returnedTypes = GetReturnedTypes(provider);
            }
            return base.EditValue(context, provider, value);
        }

        private Type[] GetReturnedTypes(IServiceProvider provider)
        {
            return new Type[] 
            {    
                typeof(DBFormItemBase),
            };
        }

        protected override string GetDisplayText(object value)
        {
            if (value != null)
                return value.ToString();
            else
                return base.GetDisplayText(value);
        }

        protected override object CreateInstance(Type itemType)
        {
            DBFormItemBase dbItem = null;
            DBForm dbForm = FindDBForm();
            if (dbForm != null)
                dbItem = dbForm.CreateInstance();

            return dbItem;
        }

        protected override void DestroyInstance(object instance)
        {
            DBForm dbForm = FindDBForm();
            if (instance is DBFormItemBase)
                dbForm.DestroyControl(instance as DBFormItemBase);                
            base.DestroyInstance(instance);
        }

        protected override CollectionForm CreateCollectionForm()
        {
            CollectionForm form = base.CreateCollectionForm();
            form.Text = "Редактор списка элементов";
            form.FormClosed += new FormClosedEventHandler(form_FormClosed);

            // Перебираем все компоненты формы
            foreach (Control control in form.Controls)
            {
                foreach (Control control1 in control.Controls)
                {
                    // Нашли Редактор Свойств
                    if (control1.GetType().ToString() ==
                       "System.Windows.Forms.Design.VsPropertyGrid")
                    {
                        // Включаем отображение описаний свойств
                        ((PropertyGrid)control1).HelpVisible = true;
                        ((PropertyGrid)control1).HelpBackColor = SystemColors.Info;
                    }
                }
            }

            return form;
        }

        void form_FormClosed(object sender, FormClosedEventArgs e)
        {
            //При закрытие формы уведомить компонент о необходимости обновления данных.
            DBForm dbForm = FindDBForm();
            if (dbForm != null)
                dbForm.OnChangedItems(this, EventArgs.Empty);
        }
    }
}
