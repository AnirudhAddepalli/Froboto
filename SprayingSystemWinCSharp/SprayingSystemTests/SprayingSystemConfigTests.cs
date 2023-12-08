namespace SprayingSystemTests
{
    [TestClass]
    public class SprayingSystemConfigTests
    {
        [TestMethod]
        public void FileNotFound()
        {
            var filename = @"c:\temp\blahblahblah.json";

            var config = SprayingSystem.Models.SprayingSystemConfig.LoadSettings(filename);

            Assert.IsNull(config);
        }

        [TestMethod]
        public void DefaultFileFound()
        {
            var filename = @"C:\repo_github\VenkataIdsCamera\VenkataIdsCamera\SprayingSystemConfig.json";

            var config = SprayingSystem.Models.SprayingSystemConfig.LoadSettings(filename);

            Assert.IsNotNull(config);
        }

        [TestMethod]
        public void BadSpeedValue()
        {
            var filename = @"C:\repo_github\VenkataIdsCamera\SprayingSystemTests\TestConfigFiles\BadSpeed.json";

            var config = SprayingSystem.Models.SprayingSystemConfig.LoadSettings(filename);

            Assert.IsNull(config);
        }

        [TestMethod]
        public void SpeedAboveRange()
        {
            var filename = @"C:\repo_github\VenkataIdsCamera\SprayingSystemTests\TestConfigFiles\SpeedAboveRange.json";

            var config = SprayingSystem.Models.SprayingSystemConfig.LoadSettings(filename);
            var isValid = SprayingSystem.Models.SprayingSystemConfigValidate.IsValid(config);

            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void SpeedBelowRange()
        {
            var filename = @"C:\repo_github\VenkataIdsCamera\SprayingSystemTests\TestConfigFiles\SpeedBelowRange.json";

            var config = SprayingSystem.Models.SprayingSystemConfig.LoadSettings(filename);
            var isValid = SprayingSystem.Models.SprayingSystemConfigValidate.IsValid(config);

            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void MotionTypeInvalid()
        {
            var filename = @"C:\repo_github\VenkataIdsCamera\SprayingSystemTests\TestConfigFiles\MotionTypeInvalid.json";

            var config = SprayingSystem.Models.SprayingSystemConfig.LoadSettings(filename);
            var isValid = SprayingSystem.Models.SprayingSystemConfigValidate.IsValid(config);

            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void MotionTypeNotProvided()
        {
            var filename = @"C:\repo_github\VenkataIdsCamera\SprayingSystemTests\TestConfigFiles\MotionTypeNotProvided.json";

            var config = SprayingSystem.Models.SprayingSystemConfig.LoadSettings(filename);
            var isValid = SprayingSystem.Models.SprayingSystemConfigValidate.IsValid(config);

            Assert.IsTrue(isValid);
        }
    }
}