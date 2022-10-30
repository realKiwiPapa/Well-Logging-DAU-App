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
using System.Windows.Shapes;
using Logging_App.Model;
using System.Collections.ObjectModel;
using System.Data;
using Logging_App.Utility;

namespace Logging_App
{
    /// <summary>
    /// StaticWorkflowPage.xaml 的交互逻辑
    /// </summary>
    public partial class StaticWorkflowPage : Page
    {
        public StaticWorkflowPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SEARCH_FLOW.DataContext = new Model.SYS_STATIC_WORK_FLOW();
            LoadStaticWorkflow();
        }

        private void New_Workflow_Checked(object sender, RoutedEventArgs e)
        {
            STATIC_WORK_FLOW.DataContext = new Model.SYS_STATIC_WORK_FLOW();
        }

        private void Save_Workflow_Click(object sender, RoutedEventArgs e)
        {
            var model = STATIC_WORK_FLOW.DataContext as Model.SYS_STATIC_WORK_FLOW;
            if (model == null || model.HasError()) throw new Exception("参数有误，请检查！");
            model.CREATE_NAME = MyHomePage.ActiveUser.COL_LOGINNAME;
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                if (dataser.Save_Static_Work_Flow(Utility.ModelHelper.SerializeObject(model)))
                {
                    LoadStaticWorkflow();
                    STATIC_WORK_FLOW.DataContext = null;
                    MessageBox.Show("保存成功");
                }
                else
                {
                    MessageBox.Show("保存失败");
                }
            }
        }
        private void LoadStaticWorkflow()
        {
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                dataGrid1.ItemsSource = dataser.GetStaticWorkflowList(Utility.ModelHelper.SerializeObject(SEARCH_FLOW.DataContext)).Tables[0].GetDefaultView();
            }
            Button_NewJobInfo.IsChecked = true;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            LoadStaticWorkflow();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var w = new SelectUserWindow1();
            w.allowMultiple = true;
            w.Owner = System.Windows.Application.Current.MainWindow;
            w.ShowDialog();
            if (!string.IsNullOrEmpty(w.LoginName))
            {
                inputer.Text = w.LoginName;
                inputer_id.Text = w.LoginID;
                flow_node_num.Text = w.LoginName.Split(',').Length.ToString();
            }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var row = dataGrid1.SelectedItem as DataRowView;
            if (row == null) return;
            string flow_id = row.Row.FieldEx<string>("FLOW_ID");
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                STATIC_WORK_FLOW.DataContext = Utility.ModelHandler<Model.SYS_STATIC_WORK_FLOW>.FillModel(dataser.GetStaticWorkflow(flow_id).Tables[0].Rows[0]);  
            }
            Button_ChangeJobInfo.IsChecked = true;
        }
    }
}
