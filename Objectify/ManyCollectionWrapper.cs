using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objectify
{
    class ManyCollectionWrapper<T> : ICollection<T>
    {
        public ICollection<T> WrappedCollection { get; set; }
        public event Action<T> OnItemAdded = delegate { };
        public event Action<T> OnItemRemoved = delegate { }; 

        public ManyCollectionWrapper(ICollection<T> wrappedCollection)
        {
            WrappedCollection = wrappedCollection;
        }

        public bool DirtyAdd(T item)
        {
            if (Contains(item))
            {
                return false;
            }
            WrappedCollection.Add(item);
            return true;
        }

        public void DirtyRemove(T item)
        {
            WrappedCollection.Remove(item);
        }

        public void Add(T item)
        {
            if (DirtyAdd(item))
            {
                OnItemAdded(item);
            }
        }

        public bool Remove(T item)
        {
            var isRemoved = WrappedCollection.Remove(item);
            if (isRemoved)
            {
                OnItemRemoved(item);
            }
            return isRemoved;
        }

        public void Clear()
        {
            var items = WrappedCollection.ToList();
            WrappedCollection.Clear();
            items.ForEach(OnItemRemoved ?? delegate { });
        }

        public IEnumerator<T> GetEnumerator()
        {
            return WrappedCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return WrappedCollection.GetEnumerator();
        }

        public bool Contains(T item)
        {
            return WrappedCollection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            WrappedCollection.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return WrappedCollection.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }
    }
}
