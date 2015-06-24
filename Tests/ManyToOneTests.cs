using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.TestModel;

namespace Tests
{
    [TestClass]
    public class ManyToOneTests
    {
        private User user1;
        private User user2;
        private User user3;
        private Blog blog1;
        private Blog blog2;
        private Blog blog3;
        private Blog blog4;

        [TestInitialize]
        public void Init()
        {
            user1 = new User("Ivo", "Katunarić", 25);
            user2 = new User("Ivo", "Katunarić", 25);
            user3 = new User("Ivo", "Katunarić", 25);

            blog1 = new Blog("Test blog1", "This is sie text of my blog1");
            blog2 = new Blog("Test blog1", "This is sie text of my blog1");
            blog3 = new Blog("Test blog1", "This is sie text of my blog1");
            blog4 = new Blog("Test blog1", "This is sie text of my blog1");
        }

        [TestMethod]
        public void SetAuthor()
        {
            blog1.Author = user1;

            Assert.AreEqual(blog1.Author, user1);
            Assert.IsTrue(user1.Blogs.Count == 1);
            Assert.AreEqual(blog1, user1.Blogs.FirstOrDefault());
        }

        [TestMethod]
        public void SetAuthorMultipleTimes()
        {
            blog1.Author = user1;
            blog2.Author = user1;
            blog3.Author = user1;

            Assert.AreEqual(blog2.Author, user1);
            Assert.IsTrue(user1.Blogs.Count == 3);
        }

        [TestMethod]
        public void SetAndUnsetAuthor()
        {
            blog1.Author = user1;
            blog2.Author = user1;
            blog3.Author = user1;
            
            blog2.Author = null;

            Assert.AreEqual(blog1.Author, user1);
            Assert.IsNull(blog2.Author);
            Assert.AreEqual(blog3.Author, user1);
            Assert.IsTrue(user1.Blogs.Count == 2);
        }

        [TestMethod]
        public void SetAuthorsAndClearBlogs()
        {
            blog1.Author = user1;
            blog2.Author = user1;
            blog3.Author = user1;

            user1.Blogs.Clear();

            Assert.IsNull(blog1.Author);
            Assert.IsNull(blog2.Author);
            Assert.IsNull(blog3.Author);
            Assert.IsTrue(user1.Blogs.Count == 0);
        }

        [TestMethod]
        public void AddSingleBlog()
        {
            user1.Blogs.Add(blog1);

            Assert.AreEqual(blog1.Author, user1, "Blog's reference is not set");
            Assert.AreEqual(user1.Blogs.First(), blog1);
            Assert.IsTrue(user1.Blogs.Count == 1);
        }

        [TestMethod]
        public void AddAndRemoveSingleBlog()
        {
            user1.Blogs.Add(blog1);
            user1.Blogs.Remove(blog1);

            Assert.IsTrue(user1.Blogs.Count == 0);
            Assert.IsTrue(blog1.Author == null);
        }

        [TestMethod]
        public void AddMultipleBlogs()
        {
            user1.Blogs.Add(blog1);
            user1.Blogs.Add(blog2);
            user1.Blogs.Add(blog3);
            user1.Blogs.Add(blog4);

            Assert.IsTrue(user1.Blogs.Count == 4);
            Assert.AreEqual(blog1.Author, user1);
            Assert.AreEqual(blog2.Author, user1);
            Assert.AreEqual(blog3.Author, user1);
            Assert.AreEqual(blog4.Author, user1);
        }

        [TestMethod]
        public void AddAndRemoveMultipleBlogs()
        {
            user1.Blogs.Add(blog1);
            user1.Blogs.Add(blog2);
            user1.Blogs.Remove(blog1);
            Assert.IsTrue(user1.Blogs.Count == 1);
            Assert.IsNull(blog1.Author, null);
            
            user1.Blogs.Add(blog3);
            user1.Blogs.Add(blog4);
            user1.Blogs.Add(blog1);
            user1.Blogs.Remove(blog2);
            Assert.IsTrue(user1.Blogs.Count == 3);

            Assert.IsNotNull(blog1.Author);
        }

        [TestMethod]
        public void AddSameBlogToDifferentUsers()
        {
            user1.Blogs.Add(blog1);
            user2.Blogs.Add(blog1);

            Assert.IsTrue(user1.Blogs.Count == 0);
            Assert.IsTrue(user2.Blogs.Count == 1);

            Assert.AreNotEqual(blog1.Author, user1);
            Assert.AreEqual(blog1.Author, user2);
        }

        [TestMethod]
        public void SetBlogAuthor()
        {
            blog1.Author = user1;

            Assert.IsTrue(user1.Blogs.Count == 1);
        }
    }
}
