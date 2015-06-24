
namespace EmbeDB
{
    public class Blog : ModelBase<Blog>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public User Author { get; set; }

        public Blog(string title, string content)
        {
            Title = title;
            Description = content;
        }
    }
}
