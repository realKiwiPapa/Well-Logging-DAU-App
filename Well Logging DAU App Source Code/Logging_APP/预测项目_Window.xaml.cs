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
using System.Collections.ObjectModel;

using System.Data;
using Logging_App.Model;


namespace Logging_App
{
    public delegate void ChangeTextHandler_预测项目(List<Model.TreeModel> treelist);
    /// <summary>
    /// 预测项目_Window.xaml 的交互逻辑
    /// </summary>
    public partial class 预测项目_Window
    {
        public event ChangeTextHandler_预测项目 ChangeTextEvent;

        #region 私有变量属性
        /// <summary>
        /// 控件数据
        /// </summary>
        private ObservableCollection<Model.TreeModel> _itemsSourceData;
        #endregion

        public 预测项目_Window()
        {
            InitializeComponent();
        }

        private void CreateTreeList(DataTable dt, Model.TreeModel parentTree = null,string parentId="null")
        {

            foreach (DataRow dr in dt.Select("parent_id " + (parentId == "null" ? "is " : "= ") + parentId, "log_item_type"))
            {
                Model.TreeModel tree = new Model.TreeModel();
                tree.ID =Convert.ToDecimal(dr["log_item_id"]);
                tree.Name = dr["log_item_type"].ToString();
                if (parentTree == null)
                {
                  if(items==null||items.Length<1)tree.IsExpanded = true;
                    _itemsSourceData.Add(tree);
                }
                else
                { 
                    tree.Parent = parentTree;
                    parentTree.Children.Add(tree);
                 //   tree.IsExpanded = false;
                }
                if (items!=null&&items.FirstOrDefault(o => o == tree.ID) != null)
                {
                    var treeParent = tree.Parent;
                    while (treeParent != null)
                    {
                        treeParent.IsExpanded=true;
                        treeParent = treeParent.Parent;
                    }
                    tree.IsChecked=true;
                }
                CreateTreeList(dt, tree, dr["log_item_id"].ToString());

            }
            

        }
        //List<Model.TreeModel> treeList = new List<Model.TreeModel>();
        decimal?[] items;
        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _itemsSourceData = new ObservableCollection<TreeModel>();
            items = this.DataContext as decimal?[];
            using(DataServiceHelper dataser =new DataServiceHelper())
            {
                dataser.GetlogItems();
                CreateTreeList(dataser.GetlogItems().Tables[0]);
                tvZsmTree.ItemsSource = _itemsSourceData;
            }

            #region
            //string[] st_tree = { "工程测井", "裸眼测井", "生产测井" };
            //string[] st_tree1_child1 = { "固井质量检测", "套损检测", "其它" };
            //string[] st_tree1_child1_child1 = { "声波检测", "电法检测", "放射性检测", "其它" };
            //string[] st_tree1_child1_child1_child1 = { "声幅", "声波变密度" };
            //IList<Model.TreeModel> treeList = new List<Model.TreeModel>();
            //for (int i = 0; i < st_tree.Length; i++)
            //{
            //    Model.TreeModel tree = new Model.TreeModel();
            //    tree.ID = i.ToString();
            //    tree.Name = st_tree[i].ToString();
            //    tree.IsExpanded = true;
            //    //if (i == 0)
            //    {
            //        for (int j = 0; j < st_tree1_child1.Length; j++)
            //        {
            //            Model.TreeModel child = new Model.TreeModel();
            //            child.ID = tree.ID + "-" + j.ToString();
            //            child.Name = st_tree1_child1[j].ToString();
            //            child.Parent = tree;
            //            tree.Children.Add(child);
            //            child.IsExpanded = false;
            //            //if (j == 0)
            //            {
            //                for (int m = 0; m < st_tree1_child1_child1.Length; m++)
            //                {
            //                    Model.TreeModel child_child = new Model.TreeModel();
            //                    child_child.ID = child.ID + "-" + m.ToString();
            //                    child_child.Name = st_tree1_child1_child1[m].ToString();
            //                    child_child.Parent = child;
            //                    child.Children.Add(child_child);
            //                    child_child.IsExpanded = false;
            //                    //if (m == 0)
            //                    {
            //                        for (int n = 0; n < st_tree1_child1_child1_child1.Length; n++)
            //                        {
            //                            Model.TreeModel child_child_child = new Model.TreeModel();
            //                            child_child_child.ID = child_child.ID + "-" + n.ToString();
            //                            child_child_child.Name = st_tree1_child1_child1_child1[n].ToString();
            //                            child_child_child.Parent = child_child;
            //                            child_child.Children.Add(child_child_child);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    treeList.Add(tree);
            //}
            //_itemsSourceData = treeList;
            //tvZsmTree.ItemsSource = treeList;
            #endregion
        }

        #region
        /// <summary>
        /// 控件数据
        /// </summary>
        public ObservableCollection<Model.TreeModel> ItemsSourceData
        {
            get { return _itemsSourceData; }
            set
            {
                _itemsSourceData = value;
                tvZsmTree.ItemsSource = _itemsSourceData;
            }
        }

        /// <summary>
        /// 设置对应Id的项为选中状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int SetCheckedById(string id, IList<Model.TreeModel> treeList)
        {
            foreach (var tree in treeList)
            {
                if (tree.ID.Equals(id))
                {
                    tree.IsChecked = true;
                    return 1;
                }
                if (SetCheckedById(id, tree.Children) == 1)
                {
                    return 1;
                }
            }

            return 0;
        }
        /// <summary>
        /// 设置对应Id的项为选中状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int SetCheckedById(string id)
        {
            foreach (var tree in ItemsSourceData)
            {
                if (tree.ID.Equals(id))
                {
                    tree.IsChecked = true;
                    return 1;
                }
                if (SetCheckedById(id, tree.Children) == 1)
                {
                    return 1;
                }
            }

            return 0;
        }

        /// <summary>
        /// 获取选中项
        /// </summary>
        /// <returns></returns>
        public List<Model.TreeModel> CheckedItemsIgnoreRelation()
        {
            return GetCheckedItemsIgnoreRelation(_itemsSourceData);
        }

        /// <summary>
        /// 私有方法，忽略层次关系的情况下，获取选中项
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<Model.TreeModel> GetCheckedItemsIgnoreRelation(ObservableCollection<Model.TreeModel> list)
        {
            List<Model.TreeModel> treeList = new List<Model.TreeModel>();
            foreach (var tree in list)
            {
                if (tree.IsChecked)
                {
                    treeList.Add(tree);
                }
                foreach (var child in GetCheckedItemsIgnoreRelation(tree.Children))
                {
                    treeList.Add(child);
                }
            }
            return treeList;
        }

        /// <summary>
        /// 选中所有子项菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSelectAllChild_Click(object sender, RoutedEventArgs e)
        {
            if (tvZsmTree.SelectedItem != null)
            {
                Model.TreeModel tree = (Model.TreeModel)tvZsmTree.SelectedItem;
                tree.IsChecked = true;
                tree.SetChildrenChecked(true);
            }
        }

        /// <summary>
        /// 全部展开菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuExpandAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (Model.TreeModel tree in tvZsmTree.ItemsSource)
            {
                tree.IsExpanded = true;
                tree.SetChildrenExpanded(true);
            }
        }

        /// <summary>
        /// 全部折叠菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuUnExpandAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (Model.TreeModel tree in tvZsmTree.ItemsSource)
            {
                tree.IsExpanded = false;
                tree.SetChildrenExpanded(false);
            }
        }

        /// <summary>
        /// 全部选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (Model.TreeModel tree in tvZsmTree.ItemsSource)
            {
                tree.IsChecked = true;
                tree.SetChildrenChecked(true);
            }
        }

        /// <summary>
        /// 全部取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuUnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (Model.TreeModel tree in tvZsmTree.ItemsSource)
            {
                tree.IsChecked = false;
                tree.SetChildrenChecked(false);
            }
        }

        /// <summary>
        /// 鼠标右键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (item != null)
            {
                item.Focus();
                e.Handled = true;
            }
        }
        static DependencyObject VisualUpwardSearch<T>(DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
                source = VisualTreeHelper.GetParent(source);

            return source;
        }
        #endregion

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ChangeTextEvent(CheckedItemsIgnoreRelation());
            this.Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
