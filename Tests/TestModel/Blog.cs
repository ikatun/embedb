using System.Collections.Generic;
using Objectify;

namespace Tests.TestModel
{
    class Blog : ModelBase<Blog>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public User Author { get; set; }
        public ICollection<Tag> Tags { get; set; }

        static Blog()
        {
            ManyToMany(blog => blog.Tags, tag => tag.Blogs);
        }

        public Blog(string title, string content)
        {
            Title = title;
            Description = content;
        }
    }
}
