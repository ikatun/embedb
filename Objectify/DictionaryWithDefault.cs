using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objectify
{
    public class DictionaryWithDefault<TKey, TValue>
    {
        private readonly Dictionary<string, TValue> _dictionary = new Dictionary<string, TValue>();
        private readonly TValue _defaultValue;

        public DictionaryWithDefault(TValue defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public TValue this[string key]
        {
            get
            {
                TValue handler;
                return _dictionary.TryGetValue(key, out handler) ? handler : _defaultValue;
            }
            set { _dictionary[key] = value; }
        }
    }
}
