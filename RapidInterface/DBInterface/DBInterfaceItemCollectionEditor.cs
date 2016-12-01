using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RapidInterface
{
    class DBInterfaceItemCollectionEditor : CollectionEditor
    {
        /// <summary>
        /// Список подерживаемых типов.
        /// </summary>
        private Type[] returnedTypes;

        public DBInterfaceItemCollectionEditor(Type type)
            : base(type)
        {
        }

        /// <summary>
        /// Поиск базового компонента.
        /// </summary>
        /// <returns></returns>
        public DBInterface FindDBInterface()
        {
            if (Context.Instance is DBInterface)
                return (DBInterface)Context.Instance;
            else if (Context.Instance is DBInterfaceActionList)
                return ((DBInterfaceActionList)Context.Instance).DBInterface;
            else if (Context.Instance is DBInterfaceItemXPObject)
                return ((DBInterfaceItemXPObject)Context.Instance).DBInterface;
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
                returnedTypes = GetReturnedTypes();
            }
            return base.EditValue(context, provider, value);
        }

        private static Type[] GetReturnedTypes()
        {
            return new Type[] 
            {    
                typeof(DBInterfaceItemString),
                typeof(DBInterfaceItemDateTime),
                typeof(DBInterfaceItemNumeric),
                typeof(DBInterfaceItemBoolean),
                typeof(DBInterfaceItemXPObject), 
                typeof(DBInterfaceItemXPCollection)
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
            DBInterface dbInterface = FindDBInterface();
            if (dbInterface != null)
                return dbInterface.CreateInstance(itemType);
            else
                return null;
        }

        protected override void DestroyInstance(object instance)
        {
            DBInterface dbInterface = FindDBInterface();
            if (instance is DBInterfaceItemBase)
                dbInterface.DestroyInstance(instance as DBInterfaceItemBase);                
            base.DestroyInstance(instance);
        }

        protected override CollectionForm CreateCollectionForm()
        {
            CollectionForm form = base.CreateCollectionForm();
            form.Text = "Редактор списка элементов";
            form.FormClosed += form_FormClosed;

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
            DBInterface dbInterface = FindDBInterface();
            if (dbInterface != null)
                dbInterface.InvokeItemsChanged(this, EventArgs.Empty);
        }
    }
}
