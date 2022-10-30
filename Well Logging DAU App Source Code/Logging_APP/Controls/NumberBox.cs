using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;

namespace Logging_App.Controls
{
    #region
    [TemplatePart(Name = "NumberBox", Type = typeof(NumberBox))]
    public sealed class NumberBox : TextBox
    {

        static NumberBox()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(NumberBox), new FrameworkPropertyMetadata(typeof(NumberBox)));
        }

        private bool onlyPositiveInteger = false;
        /// <summary>
        /// 只能是正整数
        /// </summary>
        public bool OnlyPositiveInteger
        {
            get { return onlyPositiveInteger; }
            set { onlyPositiveInteger = value; }
        }

        public decimal? DecValue
        {
            get { return (decimal?)GetValue(DecValueProperty); }
            set { SetValue(DecValueProperty, value); }
        }

        private static readonly DependencyProperty DecValueProperty =
       DependencyProperty.Register("DecValue", typeof(decimal?), typeof(NumberBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DecValueChangedCallback));
        //new PropertyMetadata(null, DecValueChangedCallback)

        private static void DecValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumberBox numberBox = d as NumberBox;
            numberBox.Text = e.NewValue == null ? null : e.NewValue.ToString();
            //return e.NewValue;
        }

        private List<Key> keyList = new List<Key>{ Key.Subtract,Key.OemMinus,Key.Decimal,Key.OemPeriod};
        protected override void OnKeyDown(KeyEventArgs e)
        {
            
            //屏蔽非法按键
            if (onlyPositiveInteger && keyList.Contains(e.Key))
            {
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Subtract || e.Key == Key.OemMinus)
            {
                if (this.Text.Contains("-") || this.SelectionStart > 0)
                {
                    e.Handled = true;
                }
                else
                {
                    e.Handled = false;
                    base.OnKeyDown(e);
                }
                return;
            }
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Decimal)
            {
                if (this.Text.Contains(".") && e.Key == Key.Decimal)
                {
                    e.Handled = true;
                    return;
                }
                e.Handled = false;
            }
            else if (((e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.OemPeriod) && e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
            {
                if (this.Text.Contains(".") && e.Key == Key.OemPeriod)
                {
                    e.Handled = true;
                    return;
                }
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
            base.OnKeyDown(e);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            decimal decValue;
            if (decimal.TryParse(this.Text.Replace(" ", ""), out decValue))
                DecValue = decValue;
            else
                DecValue = null;

            base.OnTextChanged(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            this.Text = this.DecValue.ToString();
            base.OnLostFocus(e);
        }

        protected override void OnInitialized(EventArgs e)
        {
            //CommandBinding cut=new CommandBinding(ApplicationCommands.Cut,null,Command_Cut_CanExecuter);
            CommandBinding paste = new CommandBinding(ApplicationCommands.Paste, null, Command_Paste_CanExecute);
            //this.CommandBindings.Add(cut);
            this.CommandBindings.Add(paste);
            base.OnInitialized(e);
        }

        //private void Command_Cut_CanExecuter(object sender, CanExecuteRoutedEventArgs e)
        //{
        //    e.Handled = true;
        //}

        private void Command_Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            string ClipTxt = ((string)Clipboard.GetData("Text")).Replace(" ", "");
            int CurPos = this.SelectionStart;
            string Txt = this.Text.Remove(CurPos, this.SelectionLength);
            Txt=Txt.Insert(CurPos, ClipTxt);
            decimal decValue;
            if (OnlyPositiveInteger && (Txt.IndexOf('.') > -1 || Txt.IndexOf('-') > -1))
                e.CanExecute = false;
            else
                e.CanExecute = decimal.TryParse(Txt, out decValue);
            e.Handled = true;
        }

    }
    #endregion
}
