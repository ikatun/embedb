using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeDB
{
    public class IDGenerator
    {
        public int lastId = 0;
        public int NextId()
        {
            return ++lastId;
        }
    }
}
