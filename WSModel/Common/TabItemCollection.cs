/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TabItemCollection.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TabItemCollection.cs 129988 2009-05-11 00:55:53Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */


using System;
using System.Collections;
using System.ComponentModel;

namespace Accela.ACA.BLL.Common
{
    [ToolboxItem(false)]
    public sealed class TabItemCollection : IList
    {
        internal ArrayList TabItemList;

		#region Events

        public TabItemCollection()
        {
            TabItemList = new ArrayList();
        }

        internal TabItem this[int index]
        {
            get
            {
                return (TabItem)TabItemList[index];
            }
        }

		#endregion

		#region Methods

        public void Sort()
        {
            InternalSort(new TabComparer());
        }

        private void InternalSort(IComparer comparer)
        {
            TabItemList.Sort(comparer);
        }

        internal int Add(TabItem item)
        {
            TabItemList.Add(item);
            return TabItemList.Count - 1;
        }

		#endregion

        #region IList Member

        public int Add(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (!(item is TabItem))
            {
                throw new ArgumentException("item must be a TabItem");
            }
            return Add((TabItem)item);
        }

        public void Clear()
        {
            TabItemList.Clear();
        }

        public bool Contains(object item)
        {
            if (item == null)
            {
                return false;
            }
            return TabItemList.Contains(item as TabItem);
        }

        public int IndexOf(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (!(item is TabItem))
            {
                throw new ArgumentException("item must be a TabItem");
            }
            return TabItemList.IndexOf((TabItem)item);
        }

        public void Insert(int index, object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (!(item is TabItem))
            {
                throw new ArgumentException("item must be a TabItem");
            }
            TabItemList.Insert(index, (TabItem)item);
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Remove(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (!(item is TabItem))
            {
                throw new ArgumentException("item must be a TabItem");
            }

            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
            }
        }

        public void RemoveAt(int index)
        {
            TabItemList.RemoveAt(index);
        }

        object IList.this[int index]
        {
            get
            {
                return TabItemList[index];
            }
            set
            {
                TabItemList[index] = (TabItem)value;
            }
        }

        #endregion

        #region ICollection Member

        public void CopyTo(Array array, int index)
        {
            TabItemList.CopyTo(array, index);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Count
        {
            get { return TabItemList.Count; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsSynchronized
        {
            get { return true; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SyncRoot
        {
            get { return TabItemList.SyncRoot; }
        }

        #endregion

        #region IEnumerable Member

        public IEnumerator GetEnumerator()
        {
            return TabItemList.GetEnumerator();
        }

        #endregion
    }

    internal class TabComparer : IComparer
    {
        int IComparer.Compare(Object a, Object b)
        {
            int s1 = ((TabItem)a).Order;
            int s2 = ((TabItem)b).Order;

            return s1 - s2;
        }
    }
}
