using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace RapidInterface
{
    class HostComponent
    {
        /// <summary>
        /// Функция сравнения имен компонентов и при схожести вычленение последней отличной части.
        /// </summary>
        public static bool IsSimilar(string source, string prefix, out string postfix)
        {
            postfix = "";
            if (source.Length < prefix.Length)
                return false;

            for (int i = 0; i < prefix.Length; i++)
                if (source[i] != prefix[i])
                    return false;

            for (int i = prefix.Length; i < source.Length; i++)
                postfix += source[i];

            return true;
        }

        /// <summary>
        /// Поиск сомпонентов со схожим типом.
        /// </summary>
        public static bool FindSimilarClass(IDesignerHost host, Type componentType, IComponent exeptComponent = null)
        {
            IContainer container = host.Container;

            foreach (IComponent component in container.Components)
                if (component.GetType() == componentType && 
                    component != exeptComponent)
                    return true;

            return false;
        }

        /// <summary>
        /// Найти следущий индекс для имени, анализируя все компонты на форме.
        /// </summary>
        public static int FindNextIndex(List<string> list)
        {
            List<int> listInt = new List<int>(list.Count);

            int value;
            for (int i = 0; i < list.Count; i++)
                if (int.TryParse(list[i], out value))
                    listInt.Add(value);

            int max;
            if (listInt.Count > 0)
                max = listInt.Max();
            else
                max = 0;

            return max + 1;
        }

        /// <summary>
        /// Создание компонента по имени.
        /// </summary>
        public static IComponent CreateComponent(IDesignerHost host, Type componentType, string prefix)
        {
            IContainer container = host.Container;
            // Find count of simular components.
            string postfix;
            List<string> simulars = new List<string>();
            foreach (IComponent component in container.Components)
                if (IsSimilar(component.Site.Name, prefix, out postfix))
                    simulars.Add(postfix);

            string componentName = prefix + FindNextIndex(simulars);

            IComponent comp;
            try
            {
                comp = host.CreateComponent(componentType, componentName);
            }
            catch
            {
                comp = host.CreateComponent(componentType);
            }

            return comp;
        }

        /// <summary>
        /// Удаление компонента.
        /// </summary>
        public static void DestroyComponent(IDesignerHost host, IComponent component)
        {
            if (component != null)
            {
                host.DestroyComponent(component);
                component = null;
            }
        }
    }
}
