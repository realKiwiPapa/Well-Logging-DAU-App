using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace Logging_App.GUI
{
    public class BaseWindow : Window
    {
        public BaseWindow()
        {
            this.Style = (Style)App.Current.Resources["BaseWindowStyle"];
            WindowBehaviorHelper WBH = new WindowBehaviorHelper(this);
            WBH.RepairBehavior();
            this.Loaded += delegate
           {
               if (this.MinHeight < 150) this.MinHeight = 150;
               if (this.MinWidth < 200) this.MinWidth = 200;
               ControlTemplate baseWindowTemplate = (ControlTemplate)App.Current.Resources["BaseWindowControlTemplate"];
               if (baseWindowTemplate == null) return;
               Button MinButton = (Button)baseWindowTemplate.FindName("minBtn", this);
               MinButton.Click += delegate
               {
                   this.WindowState = WindowState.Minimized;
               };
               Button MaxButton = (Button)baseWindowTemplate.FindName("maxBtn", this);
               MaxButton.Click += delegate
               {
                   this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
               };
               Button CloseButton = (Button)baseWindowTemplate.FindName("closeButton", this);
               CloseButton.Click += delegate
               {
                   this.Close();
               };
           };

        }

        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.GetPosition(this).Y < 33 && e.LeftButton == MouseButtonState.Pressed) this.DragMove();
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnPreviewMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDoubleClick(e);
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            if (e.Device.Target != null && e.Device.Target.GetType() == typeof(TextBox))
            {
                var textBox = e.Device.Target as TextBox;
                if (this.GetType()!=typeof(Logging_App.editWindow)&&!string.IsNullOrEmpty(MyHomePage.ActiveUser.COL_NAME))//&& textBox.IsEnabled && !textBox.IsReadOnly && textBox.Visibility == Visibility.Visible && textBox.ActualHeight < 300 && textBox.ActualWidth < 300
                {
                    var w = new editWindow();
                    w.Owner = this;
                    w.textbox1.Text = textBox.Text;
                    w.textbox1.IsReadOnly = textBox.IsReadOnly;
                    w.ShowDialog();
                    if (!textBox.IsReadOnly && w.isBtnOk)
                    {
                        if (textBox.AcceptsReturn)
                            textBox.Text = w.textbox1.Text;
                        else
                            textBox.Text = w.textbox1.Text.Replace("\r", "").Replace("\n", "");
                    }
                }
            }
            base.OnMouseDoubleClick(e);
        }
    }
}
