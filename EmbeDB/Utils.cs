using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeDB
{
    static class Utils
    {
        public static T GetProperty<T>(this object instance, string name)
        {
            return (T)instance.GetType().GetProperty(name).GetValue(instance, null);
        }

        public static void SetProperty(this object instance, string name, object value)
        {
            instance.GetType().GetProperty(name).SetValue(instance, value);
        }
    }
}
