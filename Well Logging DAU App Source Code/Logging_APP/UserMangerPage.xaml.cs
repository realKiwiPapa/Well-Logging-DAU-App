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
//using System.Timers;

using Logging_App.Model;
using Logging_App.Utility;

namespace Logging_App
{
    /// <summary>
    /// UserMangerPage.xaml 的交互逻辑
    /// </summary>
    public partial class UserMangerPage
    {
        public UserMangerPage()
        {
            InitializeComponent();
        }

        private DataTable GroupData;
        private DataTable RelationData;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            //var deptTypes = new Dictionary<string, decimal>();

            //foreach (int v in Enum.GetValues(typeof(Model.SYS_Enums.DeptType)))
            //{
            //    deptTypes.Add(Enum.GetName(typeof(Model.SYS_Enums.DeptType), v), (decimal)v);
            //}
            //deptTypeCombox.DisplayMemberPath = "Key";
            //deptTypeCombox.SelectedValuePath = "Value";
            //deptTypeCombox.ItemsSource = deptTypes;

            var userRole = new Utility.DataCollection<HS_ROLE>();
            foreach (UserService.UserRole v in Enum.GetValues(typeof(UserService.UserRole)))
            {
                userRole.Add(new HS_ROLE
                {
                    COL_ROLETYPE = (int)v,
                    COL_ROLENAME = Enum.GetName(typeof(UserService.UserRole), v),
                    Selected = false
                });
            }
            userRoleList.DisplayMemberPath = "COL_ROLENAME";
            userRoleList.ValueMemberPath = "COL_ROLETYPE";
            userRoleList.SelectedMemberPath = "Selected";
            userRoleList.ItemsSource = userRole;

            loadDept();
        }

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

        private void loadUser()
        {
            var dept = deptTree.SelectedItem as TreeModel;
            if (dept != null && dept.ID != null && dept.ID > 0)
            {
                using (UserServiceHelper userser = new UserServiceHelper())
                {
                    userTab.IsEnabled = false;
                    userDataGrid.ItemsSource = Utility.ModelHandler<HS_USER>.FillModel(userser.GetHSUser((decimal)dept.ID, (int)dept.TreeData));
                }
            }
            else
            {
                userDataGrid.ItemsSource = null;
            }
        }

        private void userDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = userDataGrid.SelectedItem as HS_USER;

            if (item != null)
            {
                using (UserServiceHelper userser = new UserServiceHelper())
                {
                    UserService.UserRole[] roles = userser.GetUserRole(item.COL_LOGINNAME);
                    foreach (HS_ROLE i in userRoleList.Items)
                    {
                        if (roles.Contains((UserService.UserRole)i.COL_ROLETYPE))
                        {
                            i.Selected = true;
                        }
                        else
                        {
                            i.Selected = false;
                        }
                    }
                }

                userInfo.DataContext = item.Clone();
                userTab.IsEnabled = true;
            }
        }

        private void deptTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            loadUser();
        }

        private void menuReloadTree_Click(object sender, RoutedEventArgs e)
        {
            loadDept();
        }

        private void buttonChangePassword_Click(object sender, RoutedEventArgs e)
        {
            var model = userInfo.DataContext as HS_USER;
            if (model.HasError()) throw new ArgumentException("输入有误，请检查！");
            var item = userDataGrid.SelectedItem as HS_USER;
            using (UserServiceHelper userser = new UserServiceHelper())
            {
                if (userser.ChangePassword(model.COL_ID, model.COL_PWORD))
                {
                    MessageBox.Show(App.Current.MainWindow,"修改密码成功！");
                    return;
                }
            }
            MessageBox.Show(App.Current.MainWindow,"修改密码失败！");
        }

        private void buttonChangeUserRole_Click(object sender, RoutedEventArgs e)
        {
            var model = userInfo.DataContext as HS_USER;
            using (UserServiceHelper userser = new UserServiceHelper())
            {
                UserService.UserRole[] roles = userRoleList.SelectedItems.Cast<HS_ROLE>().Select(o => (UserService.UserRole)o.COL_ROLETYPE).ToArray();
                if (userser.ChangeUserRole(model.COL_LOGINNAME, roles))
                {
                    MessageBox.Show(App.Current.MainWindow,"修改角色成功！");
                    return;
                }
            }
            MessageBox.Show(App.Current.MainWindow,"修改角色失败！");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void menuSync_Click(object sender, RoutedEventArgs e)
        {
            using (UserServiceHelper userser = new UserServiceHelper())
            {
                if (userser.SyncUser() > 0) loadDept();
            }
        }

        //private void bindDeptGroupBox(string GroupBpxHeader, SYS_DEPT dept)
        //{
        //    deptWindow.DataContext = null;
        //    deptWindow.Caption = GroupBpxHeader;
        //    deptWindow.DataContext = dept;
        //    deptWindow.Show();
        //}



        //private void menuAddDept_Click(object sender, RoutedEventArgs e)
        //{
        //    var dept = deptTree.SelectedItem as TreeModel;
        //    if (dept != null)
        //    {
        //        bindDeptGroupBox("新建单位",
        //            new SYS_DEPT
        //            {
        //                DEPT_TYPE = 0,
        //                PARENT_ID = dept.ID
        //            });
        //    }
        //}

        //private void menuEditDept_Click(object sender, RoutedEventArgs e)
        //{
        //    var dept = deptTree.SelectedItem as TreeModel;
        //    if (dept != null && dept.ID > 0)
        //    {
        //        bindDeptGroupBox("编辑单位",
        //            new SYS_DEPT
        //            {
        //                DEPT_ID = (decimal)dept.ID,
        //                NAME = dept.Name,
        //                DEPT_TYPE = (decimal)dept.TreeData,
        //                PARENT_ID = dept.Parent.ID
        //            });
        //    }
        //}

        //private void buttonSaveDept_Click(object sender, RoutedEventArgs e)
        //{
        //    var dept = deptWindow.DataContext as SYS_DEPT;
        //    var deptItem = deptTree.SelectedItem as TreeModel;
        //    if (dept == null || dept.HasError()) throw new ArgumentException("输入有误，请检查！");
        //    using (DataServiceHelper dataser =())
        //    {
        //        if (dept.DEPT_ID == null)
        //        {
        //            decimal dept_id = dataser.AddDept(Utility.ModelHelper.SerializeObject(dept));
        //            if (dept_id > 0)
        //            {
        //                TreeModel tree = new TreeModel
        //                {
        //                    ID = dept_id,
        //                    Name = dept.NAME,
        //                    Parent = deptItem,
        //                    TreeData = dept.DEPT_TYPE,
        //                    IsSelected = true
        //                };
        //                deptItem.IsExpanded = true;
        //                deptItem.Children.Add(tree);
        //                deptWindow.Close();
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            if (dataser.ChangeDept(Utility.ModelHelper.SerializeObject(dept)))
        //            {
        //                deptItem.Name = dept.NAME;
        //                deptItem.TreeData = dept.DEPT_TYPE;
        //                deptWindow.Close();
        //                return;
        //            }
        //        }
        //        throw new Exception("单位保存失败！");
        //    }
        //}

        //private void buttonCancel_Click(object sender, RoutedEventArgs e)
        //{
        //    deptWindow.Close();
        //    addUserWindow.Close();
        //}



        //private void menuAddUser_Click(object sender, RoutedEventArgs e)
        //{
        //    var dept = deptTree.SelectedItem as TreeModel;
        //    if (dept == null) return;
        //    addUserWindow.DataContext = new SYS_USER { DEPT_ID = (decimal)dept.ID };
        //    addUserWindow.Show();
        //}

        //private void buttonSaveUser_Click(object sender, RoutedEventArgs e)
        //{
        //    var user = addUserWindow.DataContext as SYS_USER;
        //    if (user == null || user.HasError()) throw new ArgumentException("输入有误，请检查！");
        //    using (DataServiceHelper dataser =())
        //    {
        //        if (dataser.CreateUser(Utility.ModelHelper.SerializeObject(user)))
        //        {
        //            MessageBox.Show("添加用户成功！");
        //            loadUser();
        //            addUserWindow.Close();
        //        }
        //        else
        //            MessageBox.Show("添加用户失败！");
        //    }

        //}

        //private void TreeViewItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    //TreeViewItem item = VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
        //    var item = sender as TreeViewItem;
        //    if (item != null && item.Header is TreeModel)
        //    {
        //        item.Focus();
        //        e.Handled = true;
        //    }
        //}

        //private void DataGridRow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    var item = sender as DataGridRow;
        //    if (!item.IsSelected || e.LeftButton == MouseButtonState.Released) return;
        //    DragDrop.DoDragDrop(userDataGrid, new DataObject(userDataGrid.SelectedItems), DragDropEffects.Copy);
        //}

        //private ToolTip moveToolTip = new ToolTip();
        //private void TreeViewItem_DragEnter(object sender, DragEventArgs e)
        //{
        //    e.Handled = true;
        //    var tree = sender as TreeViewItem;
        //    var treeModel = tree.Header as TreeModel;
        //    if (treeModel != null)
        //    {
        //        var data = e.Data.GetData("System.Windows.Controls.SelectedItemCollection") as System.Collections.IList;
        //        if (data != null)
        //        {
        //            var items = data.Cast<SYS_USER>();
        //            if (items.First().DEPT_ID != treeModel.ID)
        //            {
        //                moveToolTip.Content = string.Format("将选中的 {0} 个用户移动到 {1}", items.Count(), treeModel.Name);
        //                moveToolTip.IsOpen = true;
        //                e.Effects = DragDropEffects.Copy;
        //                return;
        //            }
        //        }
        //    }
        //    e.Effects = DragDropEffects.None;
        //    tree.AllowDrop = false;
        //}

        //private void TreeViewItem_Drop(object sender, DragEventArgs e)
        //{
        //    e.Handled = true;
        //    if (e.Effects == DragDropEffects.None) return;
        //    var tree = sender as TreeViewItem;
        //    var treeModel = tree.Header as TreeModel;
        //    var items = (e.Data.GetData("System.Windows.Controls.SelectedItemCollection") as System.Collections.IList).Cast<SYS_USER>();
        //    var usersId = items.Select(o => o.USER_ID).ToArray();
        //    moveToolTip.IsOpen = false;
        //    using (DataServiceHelper dataser =())
        //    {
        //        if (dataser.MoveUser((decimal)treeModel.ID, usersId))
        //        {
        //            tree.Focus();
        //        }
        //        else
        //        {
        //            MessageBox.Show("移动用户失败！");
        //        }
        //    }

        //}

        //private void TreeViewItem_DragLeave(object sender, DragEventArgs e)
        //{
        //    e.Handled = true;
        //    moveToolTip.IsOpen = false;
        //    var tree = sender as TreeViewItem;
        //    tree.AllowDrop = true;
        //    // tempDragTarget = sender;
        //}

        //private TreeViewItem tempItem;
        //private void BaseWindow_PreviewMouseMove(object sender, MouseEventArgs e)
        //{
        //    return;
        //    if (moveData != null)
        //    {
        //        e.Handled = true;
        //        TreeViewItem item = VisualUpwardSearch<TreeViewItem>(((UIElement)sender).InputHitTest(Mouse.GetPosition(this)) as DependencyObject) as TreeViewItem;

        //        if (item != null)
        //        {
        //            DataRowView drv = userDataGrid.SelectedItem as DataRowView;
        //            var tree = item.Header as TreeModel;
        //            if (tree != null && drv != null && drv.Row.FieldEx<decimal>("DEPT_ID") != tree.ID)
        //            {
        //                if (e.LeftButton == MouseButtonState.Pressed)
        //                {
        //                    if (tempItem != item) moveToolTip.IsOpen = false;
        //                    moveToolTip.Content = string.Format("释放鼠标左键将选中的 {0} 个用户移动到 {1}", moveData.Length, tree.Name);
        //                    moveToolTip.IsOpen = true;
        //                    Mouse.SetCursor(Cursors.Hand);
        //                    tempItem = item;
        //                    return;
        //                }
        //                else
        //                {
        //                    MoveUser();
        //                }
        //            }
        //        }
        //        Mouse.SetCursor(Cursors.No);
        //        moveToolTip.IsOpen = false;
        //        if (e.LeftButton == MouseButtonState.Released) moveData = null;
        //    }

        //}

        //private void buttonChangeUserName_Click(object sender, RoutedEventArgs e)
        //{
        //    var model = userInfo.DataContext as SYS_USER;
        //    if (model.HasError()) throw new ArgumentException("输入有误，请检查！");
        //    var item = userDataGrid.SelectedItem as SYS_USER;
        //    using (DataServiceHelper dataser =())
        //    {
        //        if (dataser.ChangeUserName(model.USER_ID, model.FULLNAME))
        //        {
        //            item.FULLNAME = model.FULLNAME;
        //            MessageBox.Show("修改名字成功！");
        //            return;
        //        }
        //    }
        //    MessageBox.Show("修改名字失败！");
        //}

    }
}
