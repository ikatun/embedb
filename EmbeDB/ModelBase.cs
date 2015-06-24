using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EmbeDB
{
    public abstract class ModelBase<T> : IIdable, IDisposable, INotifyPropertyChanged where T : ModelBase<T>
    {
        public int Id { get; private set; }
        private static readonly Repository<T> Repository;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private readonly Dictionary<string, Action<T, object, object>> _attributeChangeActions = new Dictionary<string, Action<T, object, object>>();

        static ModelBase()
        {
            Repository = new Repository<T>();
        }

        protected ModelBase()
        {
            Id = Repository.NextInt();
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

        public void SetAttributeChangeListener(string attributeName, Action<T, object, object> handler)
        {
            _attributeChangeActions[attributeName] = handler;
        }
        public void RemoveAttributeChangeListener(string attributeName)
        {
            _attributeChangeActions.Remove(attributeName);
        }

        public void OnPropertyChanged(string propertyName, object before, object after)
        {
            Console.WriteLine(GetType().Name + " with id=" + Id + " just changed " + propertyName + " from " + before + " to " + after);

            //before.GetProperty<object>(propertyName)

            if (ReferenceEquals(before, after)) return;
            if (_attributeChangeActions.ContainsKey(propertyName))
            {
                _attributeChangeActions[propertyName]((T) this, before, after);
            }

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public override bool Equals(object obj)
        {
            return obj is T && ((T) obj).Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
