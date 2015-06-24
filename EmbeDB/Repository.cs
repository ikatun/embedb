using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeDB
{
    public class Repository<T> where T : IIdable
    {
        private readonly Dictionary<int, T> _modelStorage = new Dictionary<int, T>();
        private readonly IDGenerator _idGenerator = new IDGenerator();
        
        public void Add(T obj)
        {
            _modelStorage.Add(obj.Id, obj);
        }

        public T GetById(int id)
        {
            return _modelStorage[id];
        }

        public void Remove(int id)
        {
            _modelStorage.Remove(id);
        }

        public int NextInt()
        {
            return _idGenerator.NextId();
        }
    }
}
