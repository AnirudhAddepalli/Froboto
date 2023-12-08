using System.Threading.Tasks;

namespace SprayingSystem.RpiDriver
{
    public interface IRpiDriver
    {
        Task<StandardReply> GetDateTime();
        Task<StandardReply> SetSpray();
        Task<StandardReply> SetSpray(string param);
        Task<StandardReply> SetCleanSpray(string param);
        
        Task<StandardReply> SetBlotSolenoid(string param);

        Task<StandardReply> StartRecording(string param);
        
        Task<StandardReply> GetVideoFile(string param);

        Task<StandardReply> SetLedOn();
        Task<StandardReply> SetLedOff();
    }
}
