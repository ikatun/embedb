using System;

namespace EmbeDB
{
    public class User : ModelBase<User>
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string LastName { get; set; }
        public ManyToOne<User> Blogs { get; private set; }

        public int Nesto = 22;

        public User(string name, string lastName, int age)
        {
            Name = name;
            LastName = lastName;
            Age = age;
            //Blogs = new ManyToOne<Blog, User>(b => b.Author, this);
            //Blogs = new ManyToOne.
        }

        public string FullName 
        { 
            get { return Name + LastName; }
            set { Console.WriteLine(value); }
        }
    }
}
