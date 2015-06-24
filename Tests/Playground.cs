using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Objectify;
using Tests.TestModel;

namespace Tests
{
    class Playground
    {
        class TestUser
        {
            public string Name { get; set; }
        }
        public static void Main(string[] args)
        {
            DictionaryWithDefault<string, Action> d = new DictionaryWithDefault<string, Action>(delegate { });


            d["mirko"]();
        }
    }
}
