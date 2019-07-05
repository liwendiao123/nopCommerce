using Nop.Core.Plugins;

namespace Nop.Plugin.SMS.Aliyun.Services
{

    public partial interface ISmsSender : IPlugin
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="phoneNumbers"></param>
        /// <param name="message"></param>
        bool SendSms(string phoneNumbers, string message);

        bool SendSms(string phoneNumbers, string templateCode, string templateParams = null, string outId = null);
    }
}
