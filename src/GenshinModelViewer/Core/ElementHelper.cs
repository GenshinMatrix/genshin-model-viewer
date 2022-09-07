using System;
using System.Windows;
using System.Windows.Controls;

namespace GenshinModelViewer.Core
{
    public static class ElementHelper
    {
        public static void ForEachDeep<T>(this UIElement element, Action<T> action) where T : UIElement
        {
            if (element is Panel panel)
            {
                panel.Children.ForEachDeep(action);
            }
            else if (element is Border border)
            {
                if (typeof(T) == typeof(Border))
                {
                    action(element as T);
                }
                else
                {
                    border.Child.ForEachDeep(action);
                }
            }
            else
            {
                if (element is T t)
                    action(t);
            }
        }

        public static void ForEach<T>(this UIElementCollection elements, Action<T> action) where T : UIElement
        {
            foreach (var element in elements)
            {
                if (element is T t)
                    action(t);
            }
        }

        public static void ForEachDeep<T>(this UIElementCollection elements, Action<T> action) where T : UIElement
        {
            elements.ForEach<UIElement>(element =>
            {
                element.ForEachDeep(action);
            });
        }
    }
}
