using Qc.WechatSdk.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Qc.WechatSdk
{
    public interface IWechatSdkHook
    {
        /// <summary>
        /// 获取OCR配置
        /// </summary>
        /// <returns></returns>
        WechatConfig GetConfig();
        ///// <summary>
        ///// 从缓存中获取AccessToken
        ///// </summary>
        ///// <param name="apiKey">应用接口Key</param>
        ///// <returns></returns>
        //WechatAccessTokenModel GetTokenInfo(string apiKey);
        ///// <summary>
        ///// 保存Token信息
        ///// </summary>
        ///// <returns></returns>
        //WechatAccessTokenModel SaveTokenInfo(WechatAccessTokenModel input);
        /// <summary>
        /// 从缓存中获取AccessToken信息,没有则设置
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        WechatAccessTokenModel CacheAccessToken(Func<WechatAccessTokenModel> action);
        /// <summary>
        /// 缓存jssdk票据
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="action"></param>x
        /// <returns></returns>
        string CacheApiTicket(string accessToken, Func<JsSdkApiTicketModel> action);
    }
}
