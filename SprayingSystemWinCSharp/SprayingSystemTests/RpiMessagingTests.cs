namespace SprayingSystemTests
{
    [TestClass]
    public class RpiMessagingTests
    {
        public static string rpiEndPointIp = "http://192.168.1.31:8081/";
        [TestMethod]
        public void Connection()
        {
            SprayingSystem.RpiDriver.RpiEndPoint.IpAndPort = rpiEndPointIp;
            var client = new SprayingSystem.RpiDriver.RpiDriver();

            var t = client.GetDateTime();
            var r = t.Result;

            Assert.IsNotNull(t.Status);
        }

        [TestMethod]
        public void SetCleanSpray()
        {
            SprayingSystem.RpiDriver.RpiEndPoint.IpAndPort = rpiEndPointIp;
            var client = new SprayingSystem.RpiDriver.RpiDriver();

            var parameters = $"cleantime=200,cleancycles=5";
            var t = client.SetCleanSpray(parameters);
            var r = t.Result;

            Assert.IsNotNull(r);
            Assert.IsTrue(r.Status == "OK");
        }

        [TestMethod]
        public void SetSpray()
        {
            SprayingSystem.RpiDriver.RpiEndPoint.IpAndPort = rpiEndPointIp;
            var client = new SprayingSystem.RpiDriver.RpiDriver();

            var t = client.SetSpray("duration=33");
            var r = t.Result;

            Assert.IsNotNull(r);
            Assert.IsTrue(r.Status == "OK");
        }

        [TestMethod]
        public void InvalidRequest()
        {
            SprayingSystem.RpiDriver.RpiEndPoint.IpAndPort = rpiEndPointIp;
            var client = new SprayingSystem.RpiDriver.RpiDriver();

            // TODO: Why is SetLedOn() not returning?

            var t = client.SetLedOn();
            var r = t.Result;

            Assert.IsNotNull(r);
            Assert.IsTrue(r.Status == "OK");
        }

        [TestMethod]
        public void GetVideoFile()
        {
            SprayingSystem.RpiDriver.RpiEndPoint.IpAndPort = rpiEndPointIp;
            var client = new SprayingSystem.RpiDriver.RpiDriver();

            var t = client.GetVideoFile("c:/temp/blah.mp4");
            var r = t.Result;

            Assert.IsNotNull(r);
            Assert.IsTrue(r.Status == "OK");
        }

        [TestMethod]
        public void ActuateBlotSolenoid()
        {
            SprayingSystem.RpiDriver.RpiEndPoint.IpAndPort = rpiEndPointIp;
            var rpiController = new SprayingSystem.RpiModule.RpiController();
            rpiController.Connect();

            var t = rpiController.SendBlotSolenoid("legacy",10);
            var r = t.Result;

            Assert.IsNotNull(t.Status);
        }
    }
}
