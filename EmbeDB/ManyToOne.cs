using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmbeDB
{
    public class ManyToOne<TModel> : IMany<TModel>
        where TModel : ModelBase<TModel>
    {
        private readonly HashSet<TModel> _backingContainer = new HashSet<TModel>();
        private readonly string _itemPropertyName;
        private readonly object _owner;
        private readonly string _collectionPropertyName;

        public ManyToOne(object owner, string itemPropertyName, string collectionPropertyName)
        {
            _owner = owner;
            _itemPropertyName = itemPropertyName;
            _collectionPropertyName = collectionPropertyName;
        }

        public void Add(TModel item)
        {
            _backingContainer.Add(item);

            var collectionOwner = item.GetProperty<object>(_itemPropertyName);
            if (collectionOwner != null)
            {
                collectionOwner.GetProperty<ManyToOne<TModel>>(_collectionPropertyName).RemoveWithoutResettingParentReference(item);
            }
            item.SetProperty(_itemPropertyName, _owner);
        }

        public bool Remove(TModel item)
        {
            var r = _backingContainer.Remove(item);
            item.SetProperty(_itemPropertyName, null);
            
            return r;
        }

        internal void AddWithoutSettingParentReference(TModel item)
        {
            _backingContainer.Add(item);
        }

        internal bool RemoveWithoutResettingParentReference(TModel item)
        {
            return _backingContainer.Remove(item);
        }

        public void Clear()
        {
            _backingContainer.RemoveWhere(x => true);
        }

        public bool Contains(TModel item)
        {
            return _backingContainer.Contains(item);
        }

        public void CopyTo(TModel[] array, int arrayIndex)
        {
            _backingContainer.CopyTo(array);
        }

        public int Count
        {
            get { return _backingContainer.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public IEnumerator<TModel> GetEnumerator()
        {
            return _backingContainer.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _backingContainer.GetEnumerator();
        }

        public void AddFrom(IEnumerable<TModel> source)
        {
            foreach (var item in source)
            {
                Add(item);
            }
        }
    }
}
