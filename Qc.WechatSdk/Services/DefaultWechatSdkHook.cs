using Microsoft.Extensions.Options;
using Qc.WechatSdk.Models;
using Qc.WechatSdk.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Qc.WechatSdk
{
    public class DefaultWechatSdkHook : IWechatSdkHook
    {
        private readonly WechatConfig _apiConfig;
        public DefaultWechatSdkHook(IOptions<WechatConfig> options)
        {
            _apiConfig = options.Value ?? GetConfig();
        }
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        public WechatConfig GetConfig()
        {
            return _apiConfig;
        }
        #region private
        /// <summary>
        /// 从缓存中获取AccessToken等信息
        /// </summary>
        /// <returns></returns>
        private WechatAccessTokenModel GetTokenInfo()
        {
            string readPath = Path.Combine(Path.GetFullPath(_apiConfig.SaveTokenDirPath), _apiConfig.AppId + ".txt");
            if (!File.Exists(readPath))
                return null;
            var accessResult = File.ReadAllText(readPath);
            var result = JsonHelper.Deserialize<WechatAccessTokenModel>(accessResult);
            if (result.ExpiresEndTime.HasValue && result.ExpiresEndTime.Value <= DateTime.Now)
                return null;
            return result;
        }
        /// <summary>
        /// 保存Token信息
        /// </summary>
        /// <returns></returns>
        private WechatAccessTokenModel SaveTokenInfo(WechatAccessTokenModel input)
        {
            if (!Directory.Exists(_apiConfig.SaveTokenDirPath))
            {
                Directory.CreateDirectory(_apiConfig.SaveTokenDirPath);
            }
            string savePath = Path.Combine(_apiConfig.SaveTokenDirPath, _apiConfig.AppId + ".txt");
            input.ExpiresEndTime = DateTime.Now.AddMinutes(115);
            System.IO.File.WriteAllText(savePath, JsonHelper.Serialize(input));
            return input;
        }
        #endregion
        /// <summary>
        /// 从缓存中获取AccessToken信息,没有则设置
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public WechatAccessTokenModel CacheAccessToken(Func<WechatAccessTokenModel> action)
        {
            var tokenInfo = GetTokenInfo();
            if (tokenInfo != null)
            {
                return tokenInfo;
            }
            tokenInfo = action();
            if (tokenInfo.IsError())
                return tokenInfo;

            return SaveTokenInfo(tokenInfo);
        }
        /// <summary>
        /// 缓存jssdk票据
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="action"></param>x
        /// <returns></returns>
        public string CacheApiTicket(string accessToken, Func<JsSdkApiTicketModel> action)
        {
            if (string.IsNullOrEmpty(accessToken))
                return null;
            if (!Directory.Exists(_apiConfig.SaveTokenDirPath))
                Directory.CreateDirectory(_apiConfig.SaveTokenDirPath);
            var fileName = "Ticket_" + _apiConfig.AppId + "_" + EncryptHelper.MD5Encrypt(accessToken) + ".txt";
            string filePath = Path.Combine(Path.GetFullPath(_apiConfig.SaveTokenDirPath), fileName);
            if (File.Exists(filePath))
            {
                var ticketResult = File.ReadAllText(filePath);
                var result = JsonHelper.Deserialize<JsSdkApiTicketModel>(ticketResult);
                if (result.ExpiresEndTime.HasValue && result.ExpiresEndTime.Value <= DateTime.Now)
                    return null;
                return result.Ticket;
            }
            else
            {
                var result = action();
                if (result.IsError())
                    return null;
                result.ExpiresEndTime = DateTime.Now.AddMinutes(115);
                System.IO.File.WriteAllText(filePath, JsonHelper.Serialize(result));
                return result.Ticket;
            }
        }
    }
}
