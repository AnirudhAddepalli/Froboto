using System.Diagnostics;

namespace SprayingSystemTests
{
    [TestClass]
    public class RpiMessagingSpeedTests
    {
        private static string UatIP = "http://192.168.1.3:8081/";
        private static int ITERATIONS = 1000;

        [TestMethod]
        public void ActuateBlotSolenoidL1()
        {
            // Level 1 - we test using the driver directly.

            SprayingSystem.RpiDriver.RpiEndPoint.IpAndPort = "http://192.168.1.31:8081/";
            var rpiController = new SprayingSystem.RpiModule.RpiController();
            rpiController.Connect();

            int iterations = ITERATIONS;
            long maxElapsed = 0;
            var histogram = new Histogram();

            for (int i = 0; i < iterations; i++)
            {
                var tm = new Stopwatch();
                tm.Start();

                var t = rpiController.SendBlotSolenoid("legacy",10);
                var r = t.Result;

                tm.Stop();
                long elapsed = tm.ElapsedMilliseconds;
                histogram.Add(elapsed);

                if (elapsed > maxElapsed)
                    maxElapsed = elapsed;
            }
            var stats = histogram.Stats();

            var logEntry = new List<string>();
            var logContent = $"t:{DateTime.Now}, method:ActuateBlotSolenoidL1, maxElapsed:{maxElapsed}, iterations:{iterations}, data:{histogram.Stats()}";
            logEntry.Add("{" + logContent + "}");
            File.AppendAllLines("c:/temp/RpiMessagingSpeedTest.log", logEntry);

            Assert.IsTrue(maxElapsed < 100);
        }

        [TestMethod]
        public void ActuateBlotSolenoidL2()
        {
            // We test using the controller, one level up the driver.

            SprayingSystem.RpiDriver.RpiEndPoint.IpAndPort = "http://192.168.1.31:8081/";
            //SprayingSystem.RpiDriver.RpiEndPoint.IpAndPort = "http://192.168.1.3:8081/";
            var client = new SprayingSystem.RpiDriver.RpiDriver();

            // Warm up
            //var t0 = client.SetBlotSolenoid("5");
            //var r0 = t0.Result;

            int iterations = ITERATIONS;
            long maxElapsed = 0;
            var histogram = new Histogram();

            for (int i = 0; i < iterations; i++)
            {
                var tm = new Stopwatch();
                tm.Start();

                var t = client.SetBlotSolenoid("5");
                var r = t.Result;

                tm.Stop();
                long elapsed = tm.ElapsedMilliseconds;
                histogram.Add(elapsed);

                if (elapsed > maxElapsed)
                    maxElapsed = elapsed;
            }

            var stats = histogram.Stats();

            var logEntry = new List<string>();
            var logContent = $"t:{DateTime.Now}, method:ActuateBlotSolenoidL2, maxElapsed:{maxElapsed}, iterations:{iterations}, data:{histogram.Stats()}";
            logEntry.Add("{" + logContent + "}");
            File.AppendAllLines("c:/temp/RpiMessagingSpeedTest.log", logEntry);

            Assert.IsTrue(maxElapsed < 100);
        }


        [TestMethod]
        public void ActuateBlotSolenoidL2_1msec()
        {
            // We test using the controller, one level up the driver.

            SprayingSystem.RpiDriver.RpiEndPoint.IpAndPort = "http://192.168.1.31:8081/";
            //SprayingSystem.RpiDriver.RpiEndPoint.IpAndPort = "http://192.168.1.3:8081/";
            var client = new SprayingSystem.RpiDriver.RpiDriver();

            // Warm up
            //var t0 = client.SetBlotSolenoid("5");
            //var r0 = t0.Result;

            int iterations = ITERATIONS;
            long maxElapsed = 0;
            var histogram = new Histogram();

            for (int i = 0; i < iterations; i++)
            {
                var tm = new Stopwatch();
                tm.Start();

                var t = client.SetBlotSolenoid("1");
                var r = t.Result;

                tm.Stop();
                long elapsed = tm.ElapsedMilliseconds;
                histogram.Add(elapsed);

                if (elapsed > maxElapsed)
                    maxElapsed = elapsed;
            }

            var stats = histogram.Stats();

            var logEntry = new List<string>();
            var logContent = $"t:{DateTime.Now}, method:ActuateBlotSolenoidL2_1msec, maxElapsed:{maxElapsed}, iterations:{iterations}, data:{histogram.Stats()}";
            logEntry.Add("{" + logContent + "}");
            File.AppendAllLines("c:/temp/RpiMessagingSpeedTest.log", logEntry);

            Assert.IsTrue(maxElapsed < 100);
        }

        [TestMethod]
        public void GetDateTimeSpeed()
        {
            SprayingSystem.RpiDriver.RpiEndPoint.IpAndPort = UatIP;
            var client = new SprayingSystem.RpiDriver.RpiDriver();

            // Warm up.
            var tx = client.GetDateTime();
            var rx = tx.Result;


            int iterations = ITERATIONS;
            long maxElapsed = 0;
            var histogram = new Histogram();

            for (int i = 0; i < iterations; i++)
            {
                var tm = new Stopwatch();
                tm.Start();

                var t = client.GetDateTime();
                var r = t.Result;

                tm.Stop();
                long elapsed = tm.ElapsedMilliseconds;
                histogram.Add(elapsed);

                if (elapsed > maxElapsed)
                    maxElapsed = elapsed;
            }

            var stats = histogram.Stats();

            var logEntry = new List<string>();
            var logContent = $"t:{DateTime.Now}, method:GetDateTimeSpeed, maxElapsed:{maxElapsed}, iterations:{iterations}, data:{histogram.Stats()}";
            logEntry.Add("{"+logContent+"}");
            File.AppendAllLines("c:/temp/RpiMessagingSpeedTest.log", logEntry);

            Assert.IsTrue(maxElapsed < 100);
        }
    }

    public class Histogram
    {
        private long[] h = new long[21];
        private int maxH = 20;

        private long total = 0;
        private long s2 = 0;
        private long n = 0;

        public void Add(long t)
        {
            total += t;
            s2 += t * t;
            n += 1;

            var i = t / 10;
            if (i >= maxH)
                ++h[maxH];
            else
                ++h[i];
        }

        public long Average
        {
            get { return total / n; }
        }

        public double Stdev
        {
            get { return Math.Sqrt((s2 / n) - (total / n) * (total / n)); }
        }

        public string Stats()
        {
            var data = "";
            for (int i = 0; i <= maxH; ++i)
                data += $"{i * 10}:{h[i]}, ";

            var descriptiveStats = $"n:{n}, avg:{Average}, stdev:{Stdev}";

            return "{" + data + descriptiveStats + "}";
        }
    }
}
