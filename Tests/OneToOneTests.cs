using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.TestModel;

namespace Tests
{
    [TestClass]
    public class OneToOneTests
    {
        private Blog b1;
        private Blog b2;
        private Topic t1;
        private Topic t2;

        [TestInitialize]
        public void Init()
        {
            b1 = new Blog("blabla", "blabla");
            b2 = new Blog("blbal", "ujbghu");
            t1 = new Topic("blb");
            t2 = new Topic("blabla");
        }

        [TestMethod]
        public void SetTopic()
        {
            b1.Topic = t1;

            Assert.IsNotNull(b1.Topic);
            Assert.IsNotNull(t1.Blog);
            Assert.AreSame(b1.Topic, t1);
            Assert.AreSame(t1.Blog, b1);
        }

        [TestMethod]
        public void UnsetTopic()
        {
            b1.Topic = t1;
            b1.Topic = null;

            Assert.IsNull(b1.Topic);
            Assert.IsNull(t1.Blog);
        }

        [TestMethod]
        public void ChangeTopic()
        {
            b1.Topic = t1;
            b1.Topic = t2;

            Assert.IsNull(t1.Blog);
            Assert.AreSame(b1.Topic, t2);
            Assert.AreSame(t2.Blog, b1);
        }
    }
}
