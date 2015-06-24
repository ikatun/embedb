using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objectify
{
    public interface IDirtyCollection
    {
        void DirtyAdd(ICollectionHolder x);
        bool DirtyRemove(ICollectionHolder x);
    }
}
