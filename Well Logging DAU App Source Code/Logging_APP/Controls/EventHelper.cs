using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Logging_App.Controls
{
    partial class EventHelper : ResourceDictionary
    {
        private void TextBox_PreviewDragEnter(object sender, DragEventArgs e)
        {
            var obj = e.Data.GetData(typeof(TextBox));
            if (obj == null)
                e.Data.SetData(typeof(TextBox), sender);
            else if (obj != sender)
                (sender as TextBox).Text = (obj as TextBox).Text;

        }

        private void TextBox_PreviewDrop(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }
    }
}
