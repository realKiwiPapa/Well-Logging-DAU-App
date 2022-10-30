using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Logging_App.Utility
{
    public static class ExtensionMethods
    {

        public static DependencyObject VisualSearchParent<T>(this DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
                source = VisualTreeHelper.GetParent(source);
            return source;
        }

        public static IEnumerable<T> VisualSearchAllChild<T>(this DependencyObject source, Predicate<T> filter = null) where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(source);
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(source); i++)
            {
                var childVisual = VisualTreeHelper.GetChild(source, i);
                if (childVisual is T && (filter == null || filter(childVisual as T)))
                {
                    yield return childVisual as T;
                }
                foreach (T _t in VisualSearchAllChild<T>(childVisual, filter))
                    yield return _t;
            }
        }

        public static IEnumerable<T> FindAll<T>(this IEnumerable<T> source, Predicate<T> filter) where T : class
        {
            foreach (var item in source)
                if (filter(item as T))
                    yield return item;
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source.Count() > 0)
                foreach (var item in source)
                    action(item);
        }

        public static System.Data.DataView GetDefaultView(this System.Data.DataTable dataTable)
        {
            if (dataTable == null) return null;
            return dataTable.DefaultView;
        }
    }
}
