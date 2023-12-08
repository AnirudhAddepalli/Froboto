using System.Threading.Tasks;

namespace SprayingSystem.RpiModule
{
    public interface IRpiController
    {
        bool Connect();
        
        /// <summary>
        /// Send a message and wait for reply.
        /// Throws when reply times-out.
        /// </summary>
        Task<IRpiMessageReply> SendMessage(IRpiCommand command);

        Task<IRpiMessageReply> SendSetSpray(int spray_duration_msec);

        Task<IRpiMessageReply> SendSetCleanSpray(int clean_spray_time_msec, int clean_cycles);

        Task<IRpiMessageReply> SendBlotSolenoid(string direction, int rdelay);

        Task<IRpiMessageReply> SendStartRecording(double durationSecs);

        Task<IRpiMessageReply> SendGetVideoFile(string videoParam);

        void Disconnect();
    }
}
