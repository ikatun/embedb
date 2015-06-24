using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objectify
{
    public class IdGenerator
    {
        private int _lastId = 0;
        public int NextId()
        {
            return ++_lastId;
        }
    }
}
