using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace ModelExample
{
    class Program
    {
        class TestClass
        {
            public string Name { get; set; }
        }
        static void Main(string[] args)
        {
/*
            User u1 = new User("ivo", "katunarić", 25);
            User u2 = new User("isus", "krist", 2015);
            
            Blog b1 = new Blog("little blog", "this is content", null);
            Blog b2 = new Blog("second blog", "this is more content", null);

            Console.WriteLine("-------");

            u1.Blogs.Add(b1);
            u1.Blogs.Add(b2);

            Console.WriteLine(u1.Blogs.Count);
*/
            //Console.WriteLine(u1.GetPropertyByName("Name"));
            //u1.SetPropertyByName("Name", "Mirko");

            Expression<Func<TestClass, string>> f = (x) => x.Name;

            MemberExpression ex = (MemberExpression) f.Body;
            var propertyInfo = (PropertyInfo) ex.Member;

            TestClass y = new TestClass();
            propertyInfo.SetValue(y, "Mirela");

            Console.WriteLine(propertyInfo.GetValue(y));

        }
    }
}
