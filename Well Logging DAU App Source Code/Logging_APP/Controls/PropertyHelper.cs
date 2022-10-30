using System.Windows;
using System.Windows.Input;

namespace Logging_App.Controls
{
    public  class PropertyHelper
    {
        public static readonly DependencyProperty DropCommandProperty = DependencyProperty.RegisterAttached(
            "DropCommand",
            typeof(bool),
            typeof(PropertyHelper),
            new PropertyMetadata(false, OnDropCommandChange));

        public static void SetDropCommand(DependencyObject source, ICommand value)
        {
            source.SetValue(DropCommandProperty, value);
        }

        public static ICommand GetDropCommand(DependencyObject source)
        {
            return (ICommand)source.GetValue(DropCommandProperty);
        }

        private static void OnDropCommandChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ICommand command = e.NewValue as ICommand;
            UIElement uiElement = d as UIElement;
            if (command != null && uiElement != null)
            {
                uiElement.Drop += (sender, args) => command.Execute(args.Data);
            }

            // todo: if e.OldValue is not null, detatch the handler that references it
        }
        public void TextBox_DragEnter(object sender, DragEventArgs e)
        {
           // var obj = e.Data.GetData(typeof(TextBox));
          //  if (obj == null)
          //      e.Data.SetData(typeof(TextBox), sender);
          //  else if (obj != sender)
          //      (sender as TextBox).Text = (obj as TextBox).Text;

        }
    }
}
