﻿using System;
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

namespace Logging_App.Controls
{
    /// <summary>
    /// DataPager.xaml 的交互逻辑
    /// </summary>
    public partial class DataPager : UserControl, INotifyPropertyChanged
    {
        public DataPager()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 分页前处理的事件,如果设置e.IsCancel=True将取消分页
        /// </summary>
        public event PageChangingRouteEventHandler PageChanging;
        /// <summary>
        /// 分页后处理的事件
        /// </summary>
        public event PageChangedRouteEventHandler PageChanged;

        #region 依赖属性
        /// <summary>
        /// 当前页
        /// </summary>
        public decimal PageIndex
        {
            get { return (decimal)GetValue(PageIndexProperty); }
            set { SetValue(PageIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentPage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register("PageIndex", typeof(decimal), typeof(DataPager), new UIPropertyMetadata((decimal)1, (sender, e) =>
            {
                var dp = sender as DataPager;
                dp.ChangeNavigationButtonState();
            }));


        /// <summary>
        /// 每页显示数据大小
        /// </summary>
        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); InitData(); }
        }

        // Using a DependencyProperty as the backing store for PageSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(DataPager), new UIPropertyMetadata(20, (sender, e) =>
            {
                var dp = sender as DataPager;
                if (dp == null) return;
                dp.ChangeNavigationButtonState();
            }));


        /// <summary>
        /// 记录数量
        /// </summary>
        public int TotalCount
        {
            get { return (int)GetValue(TotalCountProperty); }
            set
            {
                SetValue(TotalCountProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for TotalCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalCountProperty =
            DependencyProperty.Register("TotalCount", typeof(int), typeof(DataPager), new UIPropertyMetadata(0, (sender, e) =>
            {
                var dp = sender as DataPager;
                if (dp == null) return;
                dp.InitData();
                dp.ChangeNavigationButtonState();
            }));


        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            private set { SetValue(PageCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageCountProperty =
            DependencyProperty.Register("PageCount", typeof(int), typeof(DataPager), new UIPropertyMetadata(1));



        /// <summary>
        /// 是否可以点击首页和上一页按钮
        /// </summary>
        public bool CanGoFirstOrPrev
        {
            get
            {
                if (PageIndex <= 1) return false;
                return true;
            }
        }
        /// <summary>
        /// 是否可以点击最后页和下一页按钮
        /// </summary>
        public bool CanGoLastOrNext
        {
            get
            {
                if (PageIndex >= PageCount) return false;
                return true;
            }
        }
        #endregion
        /// <summary>
        /// 点击首页按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            OnPageChanging(1);
        }
        /// <summary>
        /// 点击上一页按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            OnPageChanging(this.PageIndex - 1);
        }
        /// <summary>
        /// 点击下一页按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            OnPageChanging(this.PageIndex + 1);
        }
        /// <summary>
        /// 点击末页按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            OnPageChanging(this.PageCount);
        }

        /// <summary>
        /// 页码更改
        /// </summary>
        /// <param name="pageIndex"></param>
        internal void OnPageChanging(decimal pageIndex)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex > this.PageCount) pageIndex = this.PageCount;
            //if (pageIndex == this.PageIndex) return;
            var oldPageIndex = this.PageIndex;
            var newPageIndex = pageIndex;
            var eventArgs = new PageChangingEventArgs() { OldPageIndex = oldPageIndex, NewPageIndex = newPageIndex };
            if (this.PageChanging != null)
            {
                this.PageChanging(this, eventArgs);
            }
            if (!eventArgs.IsCancel)
            {
                this.PageIndex = newPageIndex;
                if (this.PageChanged != null)
                {
                    this.PageChanged.Invoke(this, new PageChangedEventArgs() { CurrentPageIndex = this.PageIndex });
                }
            }
        }
        /// <summary>
        /// 通知导航按钮(首页,上一页,下一页,末页)状态的更改
        /// </summary>
        void ChangeNavigationButtonState()
        {
            this.NotifyPropertyChanged("CanGoFirstOrPrev");
            this.NotifyPropertyChanged("CanGoLastOrNext");
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitData()
        {
            if (this.TotalCount == 0)
            {
                this.PageCount = 1;
            }
            else
            {
                this.PageCount = this.TotalCount % this.PageSize > 0 ? (this.TotalCount / this.PageSize) + 1 : this.TotalCount / this.PageSize;
            }
            if (this.PageIndex < 1)
            {
                this.PageIndex = 1;
            }
            if (this.PageIndex > this.PageCount)
            {
                this.PageIndex = this.PageCount;
            }
            if (this.PageSize < 1)
            {
                this.PageSize = 20;
            }
        }


        #region INotifyPropertyChanged成员
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private void numberBox1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter||e.Key==Key.Return)
                if (numberBox1.DecValue != null)
                    OnPageChanging((decimal)numberBox1.DecValue);
        }

    }
    public delegate void PageChangingRouteEventHandler(object sender, PageChangingEventArgs e);
    public delegate void PageChangedRouteEventHandler(object sender, PageChangedEventArgs e);

    public class PageChangedEventArgs : RoutedEventArgs
    {
        public decimal CurrentPageIndex { get; set; }
    }

    public class PageChangingEventArgs : RoutedEventArgs
    {
        public decimal OldPageIndex { get; set; }
        public decimal NewPageIndex { get; set; }
        public bool IsCancel { get; set; }
    }
}
