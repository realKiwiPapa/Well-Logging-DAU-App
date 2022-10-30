using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;

namespace Logging_App.Controls
{
    /// <summary>
    /// WorkFlowControl.xaml 的交互逻辑
    /// </summary>
    public partial class WorkFlowControl : UserControl
    {
        public WorkFlowControl()
        {
            InitializeComponent();
        }

        #region WorkflowType

        private static readonly DependencyProperty WorkflowTypeProperty = DependencyProperty.Register("WorkflowType", typeof(WorkflowService.WorkflowType), typeof(UserControl));
        public WorkflowService.WorkflowType WorkflowType
        {
            get
            {
                return (WorkflowService.WorkflowType)GetValue(WorkflowTypeProperty);
            }
            set
            {
                SetValue(WorkflowTypeProperty, value);
            }
        }
        #endregion

        private static readonly DependencyProperty CanSaveProperty = DependencyProperty.Register("CanSave", typeof(bool), typeof(UserControl), new PropertyMetadata(false));
        public bool CanSave
        {
            get
            {
                return (bool)GetValue(CanSaveProperty);
            }
        }

        private static readonly DependencyProperty CanSubmitReviewProperty = DependencyProperty.Register("CanSubmitReview", typeof(bool), typeof(UserControl), new PropertyMetadata(false));
        public bool CanSubmitReview
        {
            get
            {
                return (bool)GetValue(CanSubmitReviewProperty);
            }
        }

        private static readonly DependencyProperty CanReviewProperty = DependencyProperty.Register("CanReview", typeof(bool), typeof(UserControl), new PropertyMetadata(false));
        public bool CanReview
        {
            get
            {
                return (bool)GetValue(CanReviewProperty);
            }
        }

        private static readonly DependencyProperty CanAppointDataProperty = DependencyProperty.Register("CanAppointData", typeof(bool), typeof(UserControl), new PropertyMetadata(false));
        public bool CanAppointData
        {
            get
            {
                return (bool)GetValue(CanAppointDataProperty);
            }
        }

        private static readonly DependencyProperty CanDeleteProperty = DependencyProperty.Register("CanDelete", typeof(bool), typeof(UserControl), new PropertyMetadata(false));
        public bool CanDelete
        {
            get
            {
                return (bool)GetValue(CanDeleteProperty);
            }
        }

        private static readonly DependencyProperty CanCancelSubmitReviewProperty = DependencyProperty.Register("CanCancelSubmitReview", typeof(bool), typeof(UserControl), new PropertyMetadata(false));
        public bool CanCancelSubmitReview
        {
            get
            {
                return (bool)GetValue(CanCancelSubmitReviewProperty);
            }
        }

        private static readonly DependencyProperty CanCancelReviewProperty = DependencyProperty.Register("CanCancelReview", typeof(bool), typeof(UserControl), new PropertyMetadata(false));
        public bool CanCancelReview
        {
            get
            {
                return (bool)GetValue(CanCancelReviewProperty);
            }
        }

        public WorkflowService.Controller QueryData(WorkflowService.WorkflowType type, string obj_id1, string obj_id2 = "")
        {
            using (var wfser = new WorkflowServiceHelper())
            {
                return wfser.GetWorkflowData(type, obj_id1, obj_id2);
            }
        }

        public void LoadData(string obj_id1 = "", string obj_id2 = "")
        {
            _obj_id1 = obj_id1;
            _obj_id2 = obj_id2;
            //ClearData();
            //using (var dataser = new DataServiceHelper())
            //{
            //    dataGrid1.ItemsSource = Utility.ModelHandler<Model.SYS_WORK_FLOW>.FillModel(dataser.GetWorkFlow(obj_id1, (int)this.WorkflowType));
            //}
            using (var wfser = new WorkflowServiceHelper())
            {
                var manger = wfser.GetWorkflowData(this.WorkflowType, obj_id1, obj_id2);
                SetValue(CanSaveProperty, manger.CanSave);
                SetValue(CanSubmitReviewProperty, manger.CanSubmitReview);
                SetValue(CanReviewProperty, manger.CanReview);
                SetValue(CanAppointDataProperty, manger.CanAppointData);
                SetValue(CanDeleteProperty, manger.CanDelete);
                SetValue(CanCancelSubmitReviewProperty, manger.CanCancelSubmitReview);
                SetValue(CanCancelReviewProperty, manger.CanCancelReview);
                dataGrid1.ItemsSource = manger.DataList;
            }
        }

        private string _obj_id1 = null;
        private string _obj_id2 = null;
        public void ReloadData()
        {
            if (_obj_id1 != null)
                LoadData(_obj_id1, _obj_id2);
        }

        public bool SubmitReview(string target_loginname, string obj_id1, string obj_id2 = "")
        {
            if (!string.IsNullOrWhiteSpace(target_loginname))
            {
                using (var wfser = new WorkflowServiceHelper())
                {
                    if (wfser.SubmitReview(this.WorkflowType, target_loginname, obj_id1, obj_id2))
                    {
                        MessageBox.Show(App.Current.MainWindow,"提交审核成功！");
                        ReloadData();
                        return true;
                    }
                }
            }
            return false;
        }

        public bool Review(string caption, string obj_id1, string obj_id2 = "")
        {
            var w = new MyMessageBox();
            w.Title = caption;
            w.TextContent.Text = "是否通过审核？";
            w.Owner = App.Current.MainWindow;
            w.ShowDialog();
            //var result = MessageBox.Show(App.Current.MainWindow,"是否通过审核？", caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            WorkflowService.WorkflowState state;
            switch (w.Result)
            {
                case MessageBoxResult.Yes:
                    state = WorkflowService.WorkflowState.审核通过;
                    break;
                case MessageBoxResult.No:
                    state = WorkflowService.WorkflowState.审核未通过;
                    break;
                default:
                    return false;
            }
            using (var wfser = new WorkflowServiceHelper())
            {
                if (wfser.Review(this.WorkflowType, state, obj_id1, obj_id2, w.MsgBox.Text.Trim()))
                {
                    MessageBox.Show(App.Current.MainWindow,"审核成功！");
                    ReloadData();
                    return true;
                }
                return false;
            }
        }

        public bool AppointData(string target_loginname, string obj_id1, string obj_id2 = "")
        {
            if (!string.IsNullOrWhiteSpace(target_loginname))
            {
                using (var wfser = new WorkflowServiceHelper())
                {
                    if (wfser.AppointData(this.WorkflowType, target_loginname, obj_id1, obj_id2))
                    {
                        ReloadData();
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CancelSubmitReview(string caption, string obj_id1)
        {
            if (!string.IsNullOrWhiteSpace(obj_id1))
            {
                if (MessageBox.Show(App.Current.MainWindow,"是否取消提交审核？", caption, MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                    return false;
                using (var wfser = new WorkflowServiceHelper())
                {
                    if (wfser.CancelSubmitReview(this.WorkflowType, obj_id1))
                    {
                        ReloadData();
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CancelReview(string caption, string obj_id1)
        {
            if (!string.IsNullOrWhiteSpace(obj_id1))
            {
                var w = new MyMessageBox();
                w.Title = caption;
                w.TextContent.Text = "是否退回审核？";
                w.Owner = App.Current.MainWindow;
                w.BtnNo.Visibility = Visibility.Collapsed;
                w.ShowDialog();
                if (w.Result != MessageBoxResult.Yes) return false;
                using (var wfser = new WorkflowServiceHelper())
                {
                    if (wfser.CancelReview(this.WorkflowType, obj_id1, w.MsgBox.Text.Trim()))
                    {
                        ReloadData();
                        return true;
                    }
                }
            }
            return false;
        }

        public WorkflowService.WorkflowData[] GetData()
        {
            return dataGrid1.ItemsSource as WorkflowService.WorkflowData[];
        }

        public void ClearData()
        {
            LoadData();
            //dataGrid1.ItemsSource = null;
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            //var flowStates = new Dictionary<string, int>();
            //foreach (int v in Enum.GetValues(typeof(Model.SYS_Enums.FlowState)))
            //{
            //    flowStates.Add(Enum.GetName(typeof(Model.SYS_Enums.FlowState), v), (int)v);
            //}
            //cbFlowState.DisplayMemberPath = "Key";
            //cbFlowState.SelectedValuePath = "Value";
            cbFlowState.ItemsSource = Enum.GetValues(typeof(WorkflowService.WorkflowState));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)) return;
            //LoadData();
        }
    }

    [ValueConversion(typeof(Visibility), typeof(string))]
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;
            if (string.IsNullOrEmpty(strValue))
                return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return null;
        }
    }

}
