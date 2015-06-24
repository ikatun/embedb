using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objectify
{
    public interface ICollectionHolder
    {
        void AddCollection(IDirtyCollection collection, string propertyName);
        IDirtyCollection GetCollection(string propertyName);
    }
}
