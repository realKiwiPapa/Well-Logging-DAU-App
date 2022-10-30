using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

using Logging_App.Model;

namespace Logging_App.Model
{
    public class TreeModel : ModelBase
    {
        #region 私有变量
        /// <summary>
        /// Id值
        /// </summary>
        private decimal ? _id;
        /// <summary>
        /// 显示的名称
        /// </summary>
        private string _name;
        /// <summary>
        /// 图标路径
        /// </summary>
        private string _icon;
        /// <summary>
        /// 选中状态
        /// </summary>
        private bool _isChecked;
        /// <summary>
        /// 折叠状态
        /// </summary>
        private bool _isExpanded;
        /// <summary>
        /// 子项
        /// </summary>
        private ObservableCollection<TreeModel> _children;
        /// <summary>
        /// 父项
        /// </summary>
        private TreeModel _parent;
        private bool _isSelected;
        #endregion

        /// <summary>
        /// 构造
        /// </summary>
        public TreeModel()
        {
            Children = new ObservableCollection<TreeModel>();
            _isChecked = false;
            IsExpanded = false;
            IsSelected = false;
            _icon = "/Images/16_16/folder_go.png";
        }

        /// <summary>
        /// 键值
        /// </summary>
        public decimal? ID
        {
            get { return _id; }
            set { _id = value; NotifyPropertyChanged("Id"); }
        }

        /// <summary>
        /// 显示的字符
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged("Name"); }
        }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon
        {
            get { return _icon; }
            set { _icon = value; NotifyPropertyChanged("Icon"); }
        }

        /// <summary>
        /// 指针悬停时的显示说明
        /// </summary>
        public string ToolTip
        {
            get
            {
                //return String.Format("{0}-{1}", Id, Name);
                return String.Format("{0}", Name);
            }
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                if (value != _isChecked)
                {
                    _isChecked = value;
                    NotifyPropertyChanged("IsChecked");
                    /*
                    if (_isChecked)
                    {
                        //如果选中则父项也应该选中
                        //if (Parent != null)
                        //{
                        //    Parent.IsChecked = true;
                        //}
                        //如果选中则子项也被选中
                        //if (Children != null)
                        //{
                        //    foreach (TreeModel child in Children)
                        //    {
                        //        child.IsChecked = true;
                        //    }
                        //}
                    }
                    else
                    {
                        //如果取消选中子项也应该取消选中
                        foreach (TreeModel child in Children)
                        {
                            child.IsChecked = false;
                        }
                    }*/
                }
            }
        }

        /// <summary>
        /// 是否展开
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    //折叠状态改变
                    _isExpanded = value;
                    NotifyPropertyChanged("IsExpanded");
                }
            }
        }

        /// <summary>
        /// 父项
        /// </summary>
        public TreeModel Parent
        {
            get { return _parent; }
            set { _parent = value; NotifyPropertyChanged("Parent"); }
        }

        /// <summary>
        /// 子项
        /// </summary>
        public ObservableCollection<TreeModel> Children
        {
            get { return _children; }
            set { _children = value; NotifyPropertyChanged("Children"); }
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; NotifyPropertyChanged("IsFocused"); }
        }

        /// <summary>
        /// 扩展存储的数据
        /// </summary>
        public object TreeData { set; get; }

        /// <summary>
        /// 设置所有子项的选中状态
        /// </summary>
        /// <param name="isChecked"></param>
        public void SetChildrenChecked(bool isChecked)
        {
            foreach (TreeModel child in Children)
            {
                child.IsChecked = IsChecked;
                child.SetChildrenChecked(IsChecked);
            }
        }

        /// <summary>
        /// 设置所有子项展开状态
        /// </summary>
        /// <param name="isExpanded"></param>
        public void SetChildrenExpanded(bool isExpanded)
        {
            foreach (TreeModel child in Children)
            {
                child.IsExpanded = isExpanded;
                child.SetChildrenExpanded(isExpanded);
            }
        }

    }
}
