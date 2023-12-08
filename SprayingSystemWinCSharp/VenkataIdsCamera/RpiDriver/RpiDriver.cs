using System;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SprayingSystem.RpiDriver
{
    public class RpiDriver : IRpiDriver
    {
        #region Fields

        HttpClient _client;

        readonly Uri _sprayUri = new Uri(RpiEndPoint.IpAndPort + "set-spray");
        readonly Uri _cleanSprayUri = new Uri(RpiEndPoint.IpAndPort + "set-cleanspray");
        private readonly Uri _blotSolenoidUri = new Uri(RpiEndPoint.IpAndPort + "set-blotsolenoid");

        private readonly Uri _startRecordingUri = new Uri(RpiEndPoint.IpAndPort + "start-recording");
        private readonly Uri _getVideoFile = new Uri(RpiEndPoint.IpAndPort + "get-videofile");

        readonly Uri _ledOnUri = new Uri(RpiEndPoint.IpAndPort + "set-led-on");
        readonly Uri _ledOffUri = new Uri(RpiEndPoint.IpAndPort + "set-led-off");
        private readonly Uri _getDateTime = new Uri(RpiEndPoint.IpAndPort + "get-datetime");

        #endregion

        public RpiDriver()
        {
            try
            {
                _client = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(RpiEndPoint.MessageTimeoutSecs)
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Client constructor - HttpClient object creation error: + {ex}");
            }
        }

        public async Task<StandardReply> SetSpray()
        {
            string response = await _client.GetStringAsync(_sprayUri);
            var data = JsonConvert.DeserializeObject<StandardReply>(response);
            return data;
        }

        /// <summary>
        /// A typical param value is "duration=30".
        /// </summary>
        public async Task<StandardReply> SetSpray(string param)
        {
            var paramContent = new StringContent(param);
            var response = await _client.PostAsync(_sprayUri, paramContent);
            if (response.ReasonPhrase == "OK")
            {
                var data = new StandardReply() { Status = "OK" };
                return data;
            }
            throw new Exception("Failed to spray.");
        }

        /// <summary>
        /// A typical param value is "cleantime=200,cleancycles=5"
        /// </summary>
        public async Task<StandardReply> SetCleanSpray(string param)
        {
            var paramContent = new StringContent(param);
            var response = await _client.PostAsync(_cleanSprayUri, paramContent);
            if (response.ReasonPhrase == "OK")
            {
                var data = new StandardReply() { Status = "OK" };
                return data;
            }
            throw new Exception("Failed to clean spray.");
        }

        public async Task<StandardReply> SetBlotSolenoid(string param)
        {
            var paramContent = new StringContent(param);
            var response = await _client.PostAsync(_blotSolenoidUri, paramContent);
            if (response.ReasonPhrase == "OK")
            {
                var data = new StandardReply() { Status = "OK" };
                return data;
            }
            throw new Exception("Failed to fire blot-solenoid.");
        }

        public async Task<StandardReply> SetLedOn()
        {
            string response = await _client.GetStringAsync(_ledOnUri);
            var data = JsonConvert.DeserializeObject<StandardReply>(response);
            return data;
        }

        public async Task<StandardReply> SetLedOff()
        {
            string response = await _client.GetStringAsync(_ledOffUri);
            var data = JsonConvert.DeserializeObject<StandardReply>(response);
            return data;
        }

        public async Task<StandardReply> GetDateTime()
        {
            string response = await _client.GetStringAsync(_getDateTime);
            
            //var data = JsonConvert.DeserializeObject<StandardReply>(response);
            
            var data = new StandardReply();
            data.Status = "OK";
            data.datetime = response;
            return data;
        }

        public async Task<StandardReply> StartRecording(string param)
        {
            var paramContent = new StringContent(param);
            var response = await _client.PostAsync(_startRecordingUri, paramContent);
            if (response.ReasonPhrase == "OK")
            {
                var data = new StandardReply() { Status = "OK" };
                return data;
            }
            throw new Exception("Failed to Start Recording.");
        }

        public async Task<StandardReply> GetVideoFile(string param)
        {
            var localFilename = param;

            try
            {
                using (var s = _client.GetStreamAsync(_getVideoFile))
                {
                    using (var fs = new FileStream(localFilename, FileMode.OpenOrCreate))
                    {
                        s.Result.CopyTo(fs);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get video file from RPI: " + e.Message);
            }

            var data = new StandardReply();
            data.Status = "OK";
            return data;
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static byte[] GetBytes2(string orig)
        {
            BinaryFormatter bf = new BinaryFormatter();
            byte[] bytes;
            MemoryStream ms = new MemoryStream();

            bf.Serialize(ms, orig);
            ms.Seek(0, 0);
            bytes = ms.ToArray();
            return bytes;
        }
    }
}
