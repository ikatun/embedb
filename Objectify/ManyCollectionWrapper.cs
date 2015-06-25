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
        private readonly ICollection<T> _wrappedCollection;
        private readonly Action<T> _onItemAdded;
        private readonly Action<T> _onItemRemoved;

        public ManyCollectionWrapper(ICollection<T> wrappedCollection, Action<T> onItemAdded, Action<T> onItemRemoved)
        {
            _wrappedCollection = wrappedCollection;
            _onItemAdded = onItemAdded;
            _onItemRemoved = onItemRemoved;
        }

        public void DirtyAdd(T item)
        {
            if (Contains(item))
            {
                return;
            }
            _wrappedCollection.Add(item);
        }

        public void DirtyRemove(T item)
        {
            _wrappedCollection.Remove(item);
        }

        public void Add(T item)
        {
            DirtyAdd(item);
            _onItemAdded(item);
        }

        public bool Remove(T item)
        {
            var isRemoved = _wrappedCollection.Remove(item);
            if (isRemoved)
            {
                _onItemRemoved(item);
            }
            return isRemoved;
        }

        public void Clear()
        {
            var items = _wrappedCollection.ToList();
            _wrappedCollection.Clear();
            items.ForEach(_onItemRemoved);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _wrappedCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _wrappedCollection.GetEnumerator();
        }

        public bool Contains(T item)
        {
            return _wrappedCollection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _wrappedCollection.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _wrappedCollection.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }
    }
}
