using System;
using System.Threading.Tasks;
using SprayingSystem.RpiDriver;

namespace SprayingSystem.RpiModule
{
    public class RpiController : IRpiController
    {
        private IRpiDriver _driver;

        public bool Connect()
        {
            if (_driver != null)
                return true;

            try
            {
                _driver = new RpiDriver.RpiDriver();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public async Task<IRpiMessageReply> SendMessage(IRpiCommand command)
        {
            if (command is LedCommand cmd)
            {
                if (cmd.On)
                    await _driver.SetLedOn().ConfigureAwait(true);
                else
                    await _driver.SetLedOff().ConfigureAwait(true);
                return null;
            }

            if (command is DateTimeCommand)
            {
                await _driver.GetDateTime().ConfigureAwait(true);
                return null;
            }

            await _driver.SetSpray().ConfigureAwait(true);
            return null;
        }

        public async Task<IRpiMessageReply> SendSetSpray(int spray_duration_msec)
        {
            // Example showing concatenation of multiple parameters:
            // var cmd = $"humicontrol={status}&setpoint={setPoint}&tolerance={tolerance}";
            
            var param = $"duration={spray_duration_msec}";

            var result = await _driver.SetSpray(param).ConfigureAwait(true);
            return null;
        }

        public async Task<IRpiMessageReply> SendSetCleanSpray(int cleantime=200, int cleancycles=5)
        {
            // Example showing concatenation of multiple parameters:
            // var cmd = $"humicontrol={status}&setpoint={setPoint}&tolerance={tolerance}";

            var param = $"cleantime={cleantime},cleancycles={cleancycles}";

            var result = await _driver.SetCleanSpray(param).ConfigureAwait(true);
            return null;
        }

        /// <summary>
        /// Direction can be forward or reverse
        /// </summary>
        public async Task<IRpiMessageReply> SendBlotSolenoid(string direction, int rdelay_msec)
        {
            var param = $"{direction},{rdelay_msec}";
            var result = await _driver.SetBlotSolenoid(param).ConfigureAwait(true);
            return null;
        }

        public async Task<IRpiMessageReply> SendStartRecording(double durationSecs)
        {
            var param = $"{durationSecs}";
            var reslt = await _driver.StartRecording(param).ConfigureAwait(true);
            return null;
        }

        public async Task<IRpiMessageReply> SendGetVideoFile(string videoParam)
        {
            var param = videoParam;
            var result = await _driver.GetVideoFile(videoParam);
            return null;
        }

        public void Disconnect()
        {
        }
    }
}
