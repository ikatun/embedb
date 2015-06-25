using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.TestModel;

namespace Tests
{
    [TestClass]
    public class ManyToManyTests
    {
        private Blog blog1;
        private Blog blog2;
        private Blog blog3;
        
        private Tag tag1;
        private Tag tag2;
        private Tag tag3;
        private Tag tag4;

        [TestInitialize]
        public void Init()
        {
            blog1 = new Blog("blabla1", "blablbal2");
            blog2 = new Blog("blabla1", "blablbal2");
            blog3 = new Blog("blabla1", "blablbal2");

            tag1 = new Tag("test tag 1");
            tag2 = new Tag("test tag 2");
            tag3 = new Tag("test tag 3");
            tag4 = new Tag("test tag 4");
        }

        [TestMethod]
        public void CheckIfEmpty()
        {
            Assert.IsTrue(blog1.Tags.Count == 0);
            Assert.IsTrue(tag1.Blogs.Count == 0);
        }

        [TestMethod]
        public void AddSingleTag()
        {
            blog1.Tags.Add(tag1);
            
            Assert.IsTrue(blog1.Tags.Count == 1);
            Assert.AreSame(blog1.Tags.First(), tag1);

            Assert.IsTrue(tag1.Blogs.Count == 1);
            Assert.AreSame(tag1.Blogs.First(), blog1);
        }

        [TestMethod]
        public void AddAndRemoveSingleTag()
        {
            blog1.Tags.Add(tag1);
            blog1.Tags.Remove(tag1);

            Assert.IsTrue(blog1.Tags.Count == 0);
            Assert.IsTrue(tag1.Blogs.Count == 0);
        }

        [TestMethod]
        public void AddAndRemoveMultipleTags()
        {
            blog1.Tags.Add(tag1);
            blog1.Tags.Add(tag2);
            blog1.Tags.Add(tag3);
            blog1.Tags.Add(tag4);

            blog1.Tags.Remove(tag2);
            blog1.Tags.Remove(tag1);

            Assert.IsTrue(blog1.Tags.Count == 2);
            
            Assert.IsTrue(tag1.Blogs.Count == 0);
            Assert.IsTrue(tag2.Blogs.Count == 0);
            Assert.IsTrue(tag3.Blogs.Count == 1);
            Assert.IsTrue(tag4.Blogs.Count == 1);

            Assert.AreSame(blog1.Tags.First(), tag3);
            Assert.AreSame(tag3.Blogs.First(), blog1);
        }

        [TestMethod]
        public void AddTagToMultipleBlogs()
        {
            blog1.Tags.Add(tag1);
            blog2.Tags.Add(tag1);
            blog3.Tags.Add(tag1);

            Assert.IsTrue(blog1.Tags.Count == 1);
            Assert.IsTrue(blog2.Tags.Count == 1);
            Assert.IsTrue(blog3.Tags.Count == 1);
            Assert.IsTrue(tag1.Blogs.Count == 3);

            Assert.AreSame(blog1.Tags.First(), tag1);
            Assert.AreSame(blog2.Tags.First(), tag1);
            Assert.AreSame(blog3.Tags.First(), tag1);
            
            Assert.AreSame(tag1.Blogs.First(), blog1);
            Assert.AreSame(tag1.Blogs.Skip(1).First(), blog2);
            Assert.AreSame(tag1.Blogs.Skip(2).First(), blog3);
            
        }
    }
}
