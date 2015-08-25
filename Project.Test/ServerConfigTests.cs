using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.Web.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Project.Test
{
    [TestClass]
    public class ServerConfigTests
    {
        private string CONFIG_KEY_NAME = "test_KEY";

        [TestMethod]
        public void NormalTest()
        {
            var config = new ServerConfig();
            bool res;
            var actRes = config.TryGetValue<bool>(CONFIG_KEY_NAME, out res);
            Assert.IsTrue(actRes);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void InvalidKeyTest()
        {
            var config = new ServerConfig();
            var invalidKeyName = "invalid";
            bool res;
            var actRes = config.TryGetValue<bool>(invalidKeyName, out res);
            Assert.IsFalse(actRes);
            Assert.IsFalse(res);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void InvalidTypeTest()
        {
            var config = new ServerConfig();
            int res;
            var actRes = config.TryGetValue<int>(CONFIG_KEY_NAME, out res);
        }
    }
}
