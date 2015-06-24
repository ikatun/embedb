using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Objectify
{
    public abstract class ModelBase<T> : ICollectionHolder, IIdable, IDisposable, INotifyPropertyChanged where T : ModelBase<T>
    {
        public int Id { get; private set; }
        private static readonly Repository<T> Repository = new Repository<T>();
        public event PropertyChangedEventHandler PropertyChanged = delegate { }; 

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

        private readonly IDictionary<string, IDirtyCollection> _collections = new Dictionary<string, IDirtyCollection>();

        public void AddCollection(IDirtyCollection collection, string propertyName)
        {
            _collections[propertyName] = collection;
        }

        public IDirtyCollection GetCollection(string propertyName)
        {
            return _collections[propertyName];
        }

        public void OnPropertyChanged(string propertyName, object before, object after)
        {
            Console.WriteLine(GetType().Name + " with id=" + Id + " just changed " + propertyName + " from " + before + " to " + after);

            var collection = after as IDirtyCollection;
            if (collection != null)
            {
                _collections[propertyName] = collection;
            }
            else
            {
                var modelBaseBefore = before as ICollectionHolder;
                if (modelBaseBefore != null)
                {
                    modelBaseBefore.GetCollection(propertyName).DirtyRemove(this);
                }
                var modelBaseAfter = after as ICollectionHolder;
                if (modelBaseAfter != null)
                {
                    modelBaseAfter.GetCollection(propertyName).DirtyAdd(this);
                }
            }

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
