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
using System.Data;
using System.Collections.ObjectModel;
using Logging_App.Utility;
using Logging_App.Model;

namespace Logging_App
{
    /// <summary>
    /// SelectUserWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SelectUserWindow
    {
        public SelectUserWindow()
        {
            InitializeComponent();
        }

        private HS_USER userModel;
        public HS_USER UserModel { get { return userModel; } }


        private string groupPath;
        public string GroupPath { get { return groupPath; } }

        private DataTable GroupData;
        private DataTable RelationData;

        private void CreateGroup(TreeModel parent_tree)
        {
            var rows =
                from g in GroupData.AsEnumerable()
                from r in RelationData.AsEnumerable()
                where r.FieldEx<decimal>("COL_HSITEMID") == parent_tree.ID
                    && r.FieldEx<decimal>("COL_HSITEMTYPE") == (int)parent_tree.TreeData
                    && r.FieldEx<decimal>("COL_DHSITEMID") == g.FieldEx<decimal>("COL_ID")
                select new TreeModel
                {
                    Name = g.FieldEx<string>("COL_NAME"),
                    ID = g.FieldEx<decimal>("COL_ID"),
                    Parent = parent_tree,
                    TreeData = 2
                };
            foreach (var tree in rows)
            {
                parent_tree.Children.Add(tree);
                CreateGroup(tree);
            }
        }

        private void loadDept()
        {
            ObservableCollection<TreeModel> tree = new ObservableCollection<TreeModel>();
            TreeModel rootDept = new TreeModel
            {
                Name = "组织架构",
                ID = -1,
                IsExpanded = true
            };
            tree.Add(rootDept);

            using (UserServiceHelper userser = new UserServiceHelper())
            {
                var view = userser.GetHSView().Tables[0];
                GroupData = userser.GetHSGroup().Tables[0];
                RelationData = userser.GetHSRelation().Tables[0];
                foreach (DataRow row in view.Rows)
                {
                    var child = new TreeModel
                    {
                        Name = row.FieldEx<string>("COL_NAME"),
                        ID = row.FieldEx<decimal>("COL_ID"),
                        Parent = rootDept,
                        TreeData = 4
                    };
                    rootDept.Children.Add(child);
                    CreateGroup(child);
                }
            }
            deptTree.ItemsSource = tree;
        }

        public bool IncludedMyself=false;
        private void loadUser()
        {
            var dept = deptTree.SelectedItem as TreeModel;
            if (dept != null && dept.ID != null && dept.ID > 0)
            {
                using (UserServiceHelper userser = new UserServiceHelper())
                {
                    var users = Utility.ModelHandler<HS_USER>.FillModel(userser.GetHSUser((decimal)dept.ID, (int)dept.TreeData));
                    //if (users != null)
                    //{
                    //    if (!IncludedMyself)
                    //    {
                    //        var user = users.ToList().Find(o => o.COL_LOGINNAME == MyHomePage.ActiveUser.COL_LOGINNAME);
                    //        if (user != null) users.Remove(user);
                    //    }
                    //}
                    userDataGrid.ItemsSource = users;
                }
            }
        }


        private void deptTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            loadUser();
        }

        private void menuReloadTree_Click(object sender, RoutedEventArgs e)
        {

        }

        private void userDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            loadDept();
        }

        private string GetGroupPath()
        {
            var dept = deptTree.SelectedItem as TreeModel;
            if (dept == null || dept.ID == null || dept.ID < 1) return string.Empty;
            var sb = new StringBuilder(dept.Name + "/");
            var parent = dept.Parent;
            while (parent != null && parent.ID > 0)
            {
                sb.Append(parent.Name + "/");
                parent = parent.Parent;
            }
            return sb.ToString();
        }

        private void btOk_Click(object sender, RoutedEventArgs e)
        {
            var item = userDataGrid.SelectedItem as HS_USER;
            if (item == null) return;
            userModel = item;
            groupPath = GetGroupPath();
            this.Close();
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
