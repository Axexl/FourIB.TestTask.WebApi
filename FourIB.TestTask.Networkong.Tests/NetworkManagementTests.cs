using FourIB.TestTask.Networking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FourIB.TestTask.Networkong.Tests
{
    [TestClass()]
    public class NetworkManagementTests
    {
        private NetworkManagement _networkManagement;

        #region init

        [TestInitialize()]
        public void Initialize()
        {
            _networkManagement = new NetworkManagement();
        }

        [TestCleanup()]
        public void Cleanup()
        {
            _networkManagement = null;
        }

        #endregion init

        #region Nics tests

        [TestMethod()]
        public void GetNicsTest_NotNull()
        {
            var nics = _networkManagement.GetNics();

            Assert.IsNotNull(nics);
            Assert.IsTrue(nics.Count > 0);
        }

        #endregion Nics tests

        //[TestMethod()]
        //public void GetNicsTest_CountNotZero()
        //{
        //    var nics = _networkManagement.GetNics();

        //    Assert.IsTrue(nics.Count > 0);
        //}

        //[TestMethod()]
        //public void GetNicsTest_CountDiferentUpDown()
        //{
        //    var nics = _networkManagement.GetNics();
        //    var nicsWithDown = _networkManagement.GetNics(true);

        //    Assert.IsNotNull(nics);
        //    Assert.IsNotNull(nicsWithDown);

        //    Assert.IsTrue(nicsWithDown.Count > nics.Count);
        //}

        #region work with Ip addresses test

        [TestMethod()]
        public void GetIPTest()
        {
            var nic = _networkManagement.GetNics().FirstOrDefault().Key;

            var r = _networkManagement.GetIp(nic);

            Assert.IsNotNull(r);
            Assert.IsTrue(r.Length > 0);
        }

        [TestMethod()]
        public void SetIPTest()
        {
            var nic = _networkManagement.GetNics().FirstOrDefault().Key;
            string testIp = "111.111.111.111";

            _networkManagement.SetIp(nic, testIp, "255.255.225.0");

            var newIp = _networkManagement.GetIp(nic);
            Assert.AreEqual(testIp, newIp[0]);
        }

        #endregion work with Ip addresses test

        #region SetGateway

        [TestMethod()]
        public void GetGatewayTest()
        {
            var gw = _networkManagement.GetGateway();

            Assert.IsNotNull(gw);
            Assert.IsTrue(gw.Length > 0);
            Assert.IsTrue(!string.IsNullOrEmpty(gw[0]));
        }

        //SetGateway
        [TestMethod()]
        public void SetGatewayTest()
        {
            string currentGateway = _networkManagement.GetGateway().FirstOrDefault();
            string testIp = "192.168.137.111";

            _networkManagement.SetGateway(testIp);

            var gw = _networkManagement.GetGateway();

            Assert.IsNotNull(gw);
            Assert.IsTrue(gw[0].Equals(testIp));

            _networkManagement.SetGateway(currentGateway);
        }

        #endregion SetGateway

        #region Wins tests

        //[TestMethod()]
        //public void GetWinsTest()
        //{
        //    var nic = _networkManagement.GetNics().FirstOrDefault().Key;

        //    var wins = _networkManagement.GetWins(nic);

        //    Assert.IsNotNull(wins);
        //}

        #endregion Wins tests
    }
}