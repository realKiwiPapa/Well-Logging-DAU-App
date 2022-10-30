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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Reflection;

namespace Logging_App
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(CreateMainMenu));
        }

        private void CreateMainMenu(object state)
        {
            UserService.UserRole[] userRoles;
            WorkflowService.WorkflowType[] userWorkflowTypes = null;
            using (UserServiceHelper userser = new UserServiceHelper())
            {
                userRoles = userser.GetActiveUserRoles();
            }

            using (var wfser = new WorkflowServiceHelper())
            {
                userWorkflowTypes = wfser.GetActiveUserWorkflowTypes();
            }

            var tree = new ObservableCollection<Model.TreeModel>();

            if (userRoles.Contains(UserService.UserRole.调度管理员) || userRoles.Contains(UserService.UserRole.解释管理员) || userWorkflowTypes.Contains(WorkflowService.WorkflowType.测井现场提交信息))
                tree.Add(new Model.TreeModel
                {
                    Name = "井管理",
                    IsExpanded = true,
                    Icon = "/Images/Icons/database_table.png",
                    Children = new ObservableCollection<Model.TreeModel>
                {
                    new Model.TreeModel
                    {
                        Name="井信息管理",
                        Icon="/Images/Icons/page_world.png",
                        TreeData=typeof(Well_Basic_Page)
                    },
                    new Model.TreeModel
                    {
                        
                        Name="作业项目管理",
                        Icon="/Images/Icons/page_world.png",
                        TreeData=typeof(Job_Info_Page)
                    }
                }
                });
            var children = new ObservableCollection<Model.TreeModel>();

            if (userRoles.Contains(UserService.UserRole.调度管理员) || userWorkflowTypes.Contains(WorkflowService.WorkflowType.解释处理作业))
            {
                children.Add(new Model.TreeModel
                {
                    Name = "测井任务通知单管理",
                    Icon = "/Images/Icons/page_world.png",
                    TreeData = typeof(Log_Task_Page)
                });
            }
            if (userRoles.Contains(UserService.UserRole.调度管理员) || userRoles.Contains(UserService.UserRole.解释管理员) || userWorkflowTypes.Contains(WorkflowService.WorkflowType.解释处理作业) || userWorkflowTypes.Contains(WorkflowService.WorkflowType.测井任务通知单) || userWorkflowTypes.Contains(WorkflowService.WorkflowType.计划任务书))
            {
                children.Add(new Model.TreeModel
                {
                    Name = "计划任务书管理",
                    Icon = "/Images/Icons/page_world.png",
                    TreeData = typeof(LOG_OPS_PLAN_Page)
                });
            };
            if (userRoles.Contains(UserService.UserRole.解释管理员) || userWorkflowTypes.Contains(WorkflowService.WorkflowType.测井现场提交信息) || userWorkflowTypes.Contains(WorkflowService.WorkflowType.测井任务通知单) || userWorkflowTypes.Contains(WorkflowService.WorkflowType.计划任务书))
            {
                children.Add(new Model.TreeModel
                {
                    Name = "测井作业收集信息",
                    Icon = "/Images/Icons/page_world.png",
                    TreeData = typeof(测井作业收集信息_Page)
                });
                children.Add(new Model.TreeModel
                {
                    Name = "小队施工信息",
                    Icon = "/Images/Icons/page_world.png",
                    TreeData = typeof(小队施工信息_Page)
                });
                children.Add(new Model.TreeModel
                {
                    Name = "测井现场提交信息",
                    Icon = "/Images/Icons/page_world.png",
                    TreeData = typeof(测井现场提交信息_Page)
                });
            };
            if (userRoles.Contains(UserService.UserRole.解释管理员) || userWorkflowTypes.Contains(WorkflowService.WorkflowType.测井现场提交信息))
            {
                //children.Add(new Model.TreeModel
                //   {
                //       Name = "解释成果信息",
                //       Icon = "/Images/Icons/page_world.png",
                //       TreeData = typeof(解释成果信息_Page)
                //   });
                children.Add(new Model.TreeModel
                {
                    Name = "解释处理作业",
                    Icon = "/Images/Icons/page_world.png",
                    TreeData = typeof(解释处理作业_Page)
                });
                children.Add(new Model.TreeModel
                {
                    Name = "归档入库",
                    Icon = "/Images/Icons/page_world.png",
                    TreeData = typeof(归档入库_Page)
                });
            }
            if (userRoles.Contains(UserService.UserRole.刻盘管理员))
            {
                 children.Add(new Model.TreeModel
                {
                    Name = "刻盘管理",
                    Icon = "/Images/Icons/page_world.png",
                    TreeData = typeof(Log_Dvd_Info_Page)
                });
            }
            if (children.Count > 0)
                tree.Add(new Model.TreeModel
                {
                    Name = "测井",
                    IsExpanded = true,
                    Icon = "/Images/Icons/database_table.png",
                    Children = children
                });
            if (userRoles.Contains(UserService.UserRole.调度管理员))
                tree.Add(new Model.TreeModel
                {
                    Name = "数据推送",
                    IsExpanded = true,
                    Icon = "/Images/Icons/database_table.png",
                    Children = new ObservableCollection<Model.TreeModel>
                {
                    new Model.TreeModel
                    {
                        Name="推送到A12",
                        Icon="/Images/Icons/page_world.png",
                        TreeData=typeof(DataPush)
                    },
                    new Model.TreeModel
                    {
                        
                        Name="推送到一体化",
                        Icon="/Images/Icons/page_world.png",
                        TreeData=typeof(DataPushYTHPT)
                    },
                     new Model.TreeModel
                    {
                        
                        Name="推送到西油",
                        Icon="/Images/Icons/page_world.png",
                        TreeData=typeof(DataPushXY)
                    },
                    new Model.TreeModel{
                        Name="A1测试",
                        Icon="/Images/Icons/page_world.png",
                        TreeData=typeof(A1_Page)
                    }
                }
                });
            tree.Add(new Model.TreeModel
            {
                Name = "实时测井",
                Icon = "/Images/Icons/database_table.png",
            });
            tree.Add(new Model.TreeModel
            {
                Name = "设备管理",
                Icon = "/Images/Icons/ipod.png",
            });
            tree.Add(new Model.TreeModel
            {
                Name = "统计报表",
                Icon = "/Images/Icons/report.png",
            });
            tree.Add(new Model.TreeModel
            {
                Name = "流程管理",
                Icon = "/Images/Icons/database_table.png",
                IsExpanded=true,
                Children = new ObservableCollection<Model.TreeModel> { 
                    new Model.TreeModel{
                        Name="静态流程链",
                        Icon="/Images/Icons/page_world.png",
                        TreeData=typeof(StaticWorkflowPage)
                    }
                }
            });
            if (userRoles.Contains(UserService.UserRole.批量下载员))
                tree.Add(new Model.TreeModel
                {
                    Name = "数据下载",
                    Icon = "/Images/Icons/arrow_down.png",
                    TreeData = typeof(DataDownloadPage)
                });
            showPlugin(tree);
            //if (userRoles.Contains((int)Model.SYS_Enums.UserRight.数据权限管理))
            //tree.Add(new Model.TreeModel
            //{
            //    Name = "数据权限管理",
            //    Icon = "/Images/Icons/database_edit.png",
            //    TreeData=typeof(DataRightsMangerPage)
            //});
            if (userRoles.Contains(UserService.UserRole.系统管理员))
            {
                var tree1 = new ObservableCollection<Model.TreeModel>
                {
                    new Model.TreeModel
                    {
                        Name="用户管理",
                        Icon = "/Images/Icons/folder_user.png",
                        TreeData=typeof(UserMangerPage)
                    
                    },
                    new Model.TreeModel
                    {
                        Name="在线用户",
                        Icon = "/Images/Icons/status_online.png",
                        TreeData=typeof(OnlineUserPage)
                    }
                };
                if (MyHomePage.ActiveUser.COL_LOGINNAME == "admin")
                    tree1.Add(new Model.TreeModel
                {
                    Name = "程序更新",
                    Icon = "/Images/Icons/application_get.png",
                    TreeData = typeof(AppUpdatePage)
                }
                );

                tree.Add(new Model.TreeModel
                {
                    Name = "系统管理",
                    Icon = "/Images/Icons/table_gear.png",
                    IsExpanded = true,
                    Children = tree1
                });

                tree.Add(new Model.TreeModel
                {
                    Name = "文件恢复",
                    TreeData = typeof(FileRecoveyPage)
                });
            }
#if DEBUG
            tree.Add(new Model.TreeModel
            {
                Name = "test",
                //Icon = "/Images/Icons/table_gear.png",
                TreeData = typeof(testPage)
            });
#endif
            
            this.Dispatcher.Invoke(new Action(() =>
                {
                    mainMenu.ItemsSource = tree;
                }));
        }

        private void showPlugin(ObservableCollection<Model.TreeModel> tree)
        {
            if (PluginDownload.PluginsInfo.Length > 0)
            {
                var parent = new Model.TreeModel
                  {
                      Name = "扩展",
                      Icon = "/Images/Icons/plugins.png",
                      IsExpanded = true
                  };
                foreach (var plugin in PluginDownload.PluginsInfo.OrderBy(x => x.Index))
                {
                    parent.Children.Add(new Model.TreeModel
                    {
                        Name = plugin.DisplayName,
                        Icon = "/Images/Icons/plugin.png",
                        TreeData = plugin
                    });
                }
                tree.Add(parent);
            }
        }

        private void mainMenu_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var item = e.NewValue as Model.TreeModel;
            if (item != null && item.TreeData != null)
            {
                object obj = null;
                var plugin = item.TreeData as FileService.PluginInfo;
                if (plugin != null)
                {
                    Assembly currentAssembly = Assembly.LoadFile(PluginDownload.RootPath + plugin.Name + "\\" + plugin.Name.Substring(plugin.Name.LastIndexOf('.') + 1) + ".dll");
                    var control = currentAssembly.CreateInstance(plugin.Name, true) as System.Windows.Forms.Control;
                    if (control != null)
                    {
                        var form = new System.Windows.Forms.Form();
                        form.Text = plugin.DisplayName;
                        form.MinimizeBox = false;
                        form.ClientSize = new System.Drawing.Size(control.Width, control.Height);
                        //form.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                        form.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                        form.Controls.Add(control);
                        form.ShowDialog();
                    }
                    else
                        MessageBox.Show("“" + plugin.DisplayName + "”加载失败！");
                }
                else
                {
                    Type type = Type.GetType(item.TreeData.ToString());
                    if (type.BaseType == typeof(Page))
                    {
                        obj = Activator.CreateInstance(type);
                    }
                }
                if (obj != null)
                {
                    contentTabItem.DataContext = item;
                    contentTabItem.Visibility = Visibility.Visible;
                    contentTabItem.IsSelected = true;
                    contentFrame.Navigate(obj);
                }
            }
        }

        public void OpenTask(object page)
        {
            if (page.GetType().BaseType != typeof(Page)) return;
            var tree = mainMenu.ItemsSource as ObservableCollection<Model.TreeModel>;
            var model = SearchTree(tree, page.GetType());
            if (model != null)
            {
                contentTabItem.DataContext = model;
                contentTabItem.Visibility = Visibility.Visible;
                contentTabItem.IsSelected = true;
                contentFrame.Navigate(page);
            }
        }

        private Model.TreeModel SearchTree(ObservableCollection<Model.TreeModel> tree, Type pageType)
        {
            if (tree == null) return null;
            foreach (Model.TreeModel m in tree)
            {
                if (m.TreeData != pageType)
                {
                    if (m.Children != null)
                    {
                        var m1 = SearchTree(m.Children, pageType);
                        if (m1 != null) return m1;
                    }
                }
                else
                {
                    return m;
                }
            }
            return null;
        }
    }
}
