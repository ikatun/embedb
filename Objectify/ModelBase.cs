using System;
using System.Collections;
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
        private static Action<T> _onModelConstructed = delegate { };
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
            Repository.Add((T) this);
            _onModelConstructed((T) this);
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

            ModelBase<TItem>.ChangeHandlers[itemPropertyName] += (item, oldOwner, newOwner) =>
            {
                if (oldOwner != null)
                {
                    getCollection((T) oldOwner).Remove(item);
                }
                if (newOwner != null)
                {
                    getCollection((T) newOwner).Add(item);
                }
            };

            var setCollection = collectionGetter.ToSetter();
            var setOwner = ownerGetter.ToSetter();

            _onModelConstructed += owner =>
            {
                var collection = new ManyCollectionWrapper<TItem>(new List<TItem>());
                collection.OnItemAdded += item => setOwner(item, owner);
                collection.OnItemRemoved += item => setOwner(item, null);

                setCollection(owner, collection);
            };
        }

        protected static void OneToOne<TItem>(Expression<Func<T, TItem>> f1, Expression<Func<TItem, T>> f2)
            where TItem : ModelBase<TItem>
        {
        }

        protected static void ManyToMany<TItem>(Expression<Func<T, ICollection<TItem>>> firstCollectionGetter,
            Expression<Func<TItem, ICollection<T>>> secondCollectionGetter)
            where TItem : ModelBase<TItem>
        {
            var setFistCollection = firstCollectionGetter.ToSetter();
            var getSecondCollection = secondCollectionGetter.Compile();
            
            _onModelConstructed += owner =>
            {
                var collection = new ManyCollectionWrapper<TItem>(new List<TItem>());
                setFistCollection(owner, collection);
                collection.OnItemAdded += item => getSecondCollection(item).Add(owner);
                collection.OnItemRemoved += item => getSecondCollection(item).Remove(owner);
            };

            var setSecondCollection = secondCollectionGetter.ToSetter();
            var getFirstCollection = firstCollectionGetter.Compile();

            ModelBase<TItem>._onModelConstructed += owner =>
            {
                var collection = new ManyCollectionWrapper<T>(new List<T>());
                setSecondCollection(owner, collection);
                collection.OnItemAdded += item => getFirstCollection(item).Add(owner);
                collection.OnItemRemoved += item => getFirstCollection(item).Remove(owner);
            };
        } 
    }
}
