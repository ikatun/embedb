using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Objectify
{
    public static class Utils
    {
        public static Action<TOwner, TProperty> ToSetter<TOwner, TProperty>(
            this Expression<Func<TOwner, TProperty>> getter)
        {
            var propertyInfo = ((PropertyInfo) ((MemberExpression) getter.Body).Member);
            
            var typeOfSetterAction = typeof (Action<TOwner, TProperty>);
            var setterAsDelegate = Delegate.CreateDelegate(typeOfSetterAction, propertyInfo.GetSetMethod());
            
            var setterAsAction = (Action<TOwner, TProperty>) setterAsDelegate;

            return setterAsAction;
        }

        public static string PropertyName<TOwner, TProperty>(this Expression<Func<TOwner, TProperty>> getter)
        {
            return ((MemberExpression) getter.Body).Member.Name;
        }
    }
}
