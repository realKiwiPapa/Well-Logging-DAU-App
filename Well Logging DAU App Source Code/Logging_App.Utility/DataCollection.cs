using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

using Logging_App.Model;

namespace Logging_App.Utility
{
    public class DataCollection<T> : ObservableCollection<T> where T : ModelBase, new()
    {
        private List<T> removedData = new List<T>();
        private List<T> changedData = new List<T>();

        /// <summary>
        /// 移除的数据
        /// </summary>
        public List<T> RemovedData
        {
            get { return removedData; }
        }
        /// <summary>
        /// 改变了的数据
        /// </summary>
        public List<T> ChangedData
        {
            get { return changedData; }
        }

        /// <summary>
        ///  移除一个对象，并将对象加入RemovedData中
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        new public bool Remove(T item)
        {
            if (base.Remove(item))
            {
                removedData.Add(item);
                changedData.Remove(item);
                return true;
            }
            return false;
        }

        public int RemoveAll(Predicate<T> match)
        {
            int n = 0;
            int i = 0;
            while (i < this.Count)
            {
                if (match(this[i]) && this.Remove(this[i]))
                {
                    n++;
                }
                else
                    i++;
            }
            return n;
        }

        /// <summary>
        /// 添加一个对象，此对象不会加入ChangedData中
        /// </summary>
        /// <param name="item"></param>
        new public void Add(T item)
        {
            if (this.Contains(item)) return;
            base.Add(item);
            item.PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
            removedData.Remove(item);
        }

        /// <summary>
        /// 添加一个对象，并将对象加入ChangedData中
        /// </summary>
        /// <param name="item"></param>
        public void AddNew(T item)
        {
            this.Add(item);
            changedData.Add(item);
        }

        /// <summary>
        /// 检查数据中是否存在错误
        /// </summary>
        /// <returns></returns>
        public bool HasError()
        {
            foreach (T t in changedData)
            {
                if (t.HasError()) return true;
            }
            return false;
        }

        private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var item = sender as T;
            if (!changedData.Contains(item))
                changedData.Add(item);
        }

    }
}
