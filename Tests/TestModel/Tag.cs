using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objectify;

namespace Tests.TestModel
{
    class Tag : ModelBase<Tag>
    {
        public string Name { get; set; }
        public ICollection<Blog> Blogs { get; set; }

        public Tag(string name)
        {
            Name = name;
        }
    }
}
