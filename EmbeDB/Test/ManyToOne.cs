using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Objectify
{
    public class ManyToOne<TListItem, TItemOwner> : ICollection<TListItem>, IDirtyCollection
        where TListItem : ModelBase<TListItem>, IIdable
        where TItemOwner : ModelBase<TItemOwner>, IIdable
    {
        private readonly HashSet<TListItem> _backingSet = new HashSet<TListItem>();
        private readonly PropertyInfo _propertyInfo;
        private readonly TItemOwner _owner;

        public ManyToOne(Expression<Func<TListItem, TItemOwner>> f, TItemOwner owner)
        {
            var expressionBody = (MemberExpression)f.Body;
            _propertyInfo = (PropertyInfo)expressionBody.Member;
            _owner = owner;
        }
        
        public void DirtyAdd(ICollectionHolder x)
        {
            _backingSet.Add((TListItem)x);
        }

        public bool DirtyRemove(ICollectionHolder x)
        {
            return _backingSet.Remove((TListItem)x);
        }

        public void Add(TListItem item)
        {
            _propertyInfo.SetValue(item, _owner);
        }

        public bool Remove(TListItem item)
        {
            var ret = DirtyRemove(item);
            _propertyInfo.SetValue(item, null);

            return ret;
        }

        public void Clear()
        {
            _backingSet.RemoveWhere(x => true);
        }

        public bool Contains(TListItem item)
        {
            return _backingSet.Contains(item);
        }

        public void CopyTo(TListItem[] array, int arrayIndex)
        {
            _backingSet.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _backingSet.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public IEnumerator<TListItem> GetEnumerator()
        {
            return _backingSet.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _backingSet.GetEnumerator();
        }
    }
}
