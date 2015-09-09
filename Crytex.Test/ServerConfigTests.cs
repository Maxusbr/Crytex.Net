using Microsoft.VisualStudio.TestTools.UnitTesting;
using Crytex.Web.Service;
using System;
using Crytex.Model.Exceptions;

namespace Crytex.Test
{
    [TestClass]
    public class ServerConfigTests
    {
        private string CONFIG_KEY_NAME = "test_KEY";

        [TestMethod]
        public void NormalTest()
        {
            var config = new ServerConfig();
            bool res = config.GetValue<bool>(CONFIG_KEY_NAME);
            Assert.IsTrue(res);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationConfigException))]
        public void InvalidKeyTest()
        {
            var config = new ServerConfig();
            var invalidKeyName = "invalid";
            bool res = config.GetValue<bool>(invalidKeyName);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void InvalidTypeTest()
        {
            var config = new ServerConfig();
            int res = config.GetValue<int>(CONFIG_KEY_NAME);
        }
    }
}
