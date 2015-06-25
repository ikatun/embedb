using System;
using System.Collections.Generic;
using Objectify;

namespace Tests.TestModel
{
    class User : ModelBase<User>
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string LastName { get; set; }

        public ICollection<Blog> Blogs { get; set; }
        public ICollection<Theme> Themes { get; set; }
        public ICollection<User> Friends { get; set; }

        static User()
        {
            ManyToOne(user => user.Blogs, blog => blog.Author);
            ManyToOne(user => user.Themes, theme => theme.Author);
            ManyToMany(user => user.Friends, friend => friend.Friends);
        }

        public User(string name, string lastName, int age)
        {
            Name = name;
            LastName = lastName;
            Age = age;
        }
    }
}
