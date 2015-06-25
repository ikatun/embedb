using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objectify;

namespace Tests.TestModel
{
    class Topic : ModelBase<Topic>
    {
        public string Text { get; set; }
        public Blog Blog { get; set; }

        static Topic()
        {
            OneToOne(topic => topic.Blog, blog => blog.Topic);
        }

        public Topic(string text)
        {
            Text = text;
        }
    }
}
