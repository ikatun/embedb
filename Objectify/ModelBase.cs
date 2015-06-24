using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Objectify
{
    public abstract class ModelBase<T> : IIdable, IDisposable, INotifyPropertyChanged where T : ModelBase<T>
    {
        public delegate void PropertyChangeHandler(T changingObject, object oldValue, object newValue);

        private static readonly DictionaryWithDefault<string, PropertyChangeHandler> ChangeHandlers;
        private static readonly Repository<T> Repository;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public int Id { get; private set; }

        static ModelBase()
        {
            ChangeHandlers = new DictionaryWithDefault<string, PropertyChangeHandler>(delegate { });
            Repository = new Repository<T>();
        }

        protected ModelBase()
        {
            Id = Repository.NextId();
            Repository.Add((T)this);
        }

        public static T ById(int id)
        {
            return Repository.GetById(id);
        }

        public virtual void Dispose()
        {
            Repository.Remove(Id);
        }

        public void OnPropertyChanged(string propertyName, object before, object after)
        {
            if (ReferenceEquals(before, after))
                return;

            ChangeHandlers[propertyName]((T) this, before, after);
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected static void ManyToOne<TItem>(Expression<Func<T, ICollection<TItem>>> collectionGetter,
            Expression<Func<TItem, T>> ownerGetter)
            where TItem : ModelBase<TItem>
        {
            var itemPropertyName = ownerGetter.PropertyName();
            var getCollection = collectionGetter.Compile();
            Func<object, ManyCollectionWrapper<TItem>> getCollectionWrapper =
                x => (ManyCollectionWrapper<TItem>) getCollection((T) x);

            ModelBase<TItem>.ChangeHandlers[itemPropertyName] += (item, oldOwner, newOwner) =>
            {
                if (oldOwner != null)
                {
                    getCollectionWrapper(oldOwner).DirtyRemove(item);
                }
                if (newOwner != null)
                {
                    getCollectionWrapper(newOwner).DirtyAdd(item);
                }
            };

            ChangeHandlers[collectionGetter.PropertyName()] +=
                (owner, old, @new) => WrapCollection(owner, collectionGetter.ToSetter(), old, @new, ownerGetter.ToSetter());
        }

        private static void WrapCollection<TItem>(T owner, Action<T, ICollection<TItem>> setCollection, object old, object @new, Action<TItem, T> setOwner)
        {
            if (@new is ManyCollectionWrapper<TItem>)
                return;

            if (old != null)
            {
                throw new ArgumentException("Collections reference can't be changed after being initialized.");
            }

            var collection = new ManyCollectionWrapper<TItem>((ICollection<TItem>)@new, item => setOwner(item, owner), item => setOwner(item, null));

            setCollection(owner, collection);
        }

        protected static void OneToOne<TItem>(Expression<Func<T, TItem>> f1, Expression<Func<TItem, T>> f2)
            where TItem : ModelBase<TItem>
        {
        }

        protected static void ManyToMany<TItem>(Expression<Func<T, ICollection<TItem>>> f1,
            Expression<Func<ICollection<TItem>, T>> f2)
            where TItem : ModelBase<TItem>
        {

        } 
    }
}
