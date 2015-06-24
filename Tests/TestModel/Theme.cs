using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objectify;

namespace Tests.TestModel
{
    class Theme : ModelBase<Theme>
    {
        public string Name { get; set; }
        public User Author { get; set; }

        public Theme(string name)
        {
            Name = name;
        }
    }
}
