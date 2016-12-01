using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace RapidInterface
{
    public class DBConnectionDesigner : ComponentDesigner
    {

        /// <summary>
        /// Основной компонент.
        /// </summary>
        DBConnection dbConnection { get; set; }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            dbConnection = Component as DBConnection;
        }

        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (HostComponent.FindSimilarClass(host, typeof(DBConnection), dbConnection))
            {
                XtraMessageBox.Show("Объект типа DBClass уже содержится на форме",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);

                dbConnection.DestroyItself();
            }
            else
            {
                object form = defaultValues["Parent"];
                if (form != null && (TypeEx.IsSubclassOf(form.GetType(), typeof(Form)) || form.GetType() == typeof(Form)))
                {
                    base.InitializeNewComponent(defaultValues);
                    dbConnection.OwnerForm = form as Form;
                }
                else
                {
                    XtraMessageBox.Show("Данный объект не типа Form.",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1);

                    dbConnection.DestroyItself();
                }
            }
        }
    }
}
