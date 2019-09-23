using Microsoft.Extensions.Options;
using Qc.WechatSdk.Models;
using Qc.WechatSdk.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Qc.WechatSdk
{
    public class WechatService
    {
        private readonly HttpClient _httpClient;
        private readonly WechatConfig _apiConfig;
        private readonly IWechatSdkHook _sdkHook;
        public WechatService(IHttpClientFactory _httpClientFactory
            , IWechatSdkHook sdkHook
            )
        {
            _sdkHook = sdkHook;
            _apiConfig = sdkHook.GetConfig();
            if (_apiConfig == null)
                throw new Exception("Wechat not configured");
            _httpClient = _httpClientFactory.CreateClient("Wechat");
            if (!string.IsNullOrWhiteSpace(_apiConfig.ApiUrl))
                _httpClient.BaseAddress = new Uri(_apiConfig.ApiUrl);
            if (_apiConfig.Timeout.HasValue)
                _httpClient.Timeout = TimeSpan.FromSeconds(_apiConfig.Timeout.Value);
        }

        #region 私有方法

        /// <summary>
        /// 根据Code获取访问票据
        /// </summary>
        /// <returns></returns>
        private WechatAccessTokenModel GetAccessTokenByCode(string code)
        {
            var result = _httpClient.HttpGet<WechatAccessTokenModel>($"/sns/oauth2/access_token?appid={_apiConfig.AppId}&secret={_apiConfig.Appsecret}&code={code}&grant_type=authorization_code");
            return result;
        }
        /// <summary>
        /// 刷新访问票据。
        /// </summary>
        /// <param name="refreshToken">刷新票据。</param>
        /// <returns>结果模型。</returns>
        private WechatAccessTokenModel RefreshToken(string refreshToken)
        {
            var result = _httpClient.HttpGet<WechatAccessTokenModel>($"sns/oauth2/refresh_token?appid={_apiConfig.AppId}&grant_type=refresh_token&refresh_token={refreshToken}");
            return result;
        }
        #endregion

        /// <summary>
        /// 获取访问票据
        /// </summary>
        /// <returns></returns>
        public WechatAccessTokenModel GetAccessToken()
        {
            return _sdkHook.CacheAccessToken(() =>
            {
                var result = _httpClient.HttpGet<WechatAccessTokenModel>($"/cgi-bin/token?grant_type=client_credential&appid={_apiConfig.AppId}&secret={_apiConfig.Appsecret}");
                return result;
            });
        }
        /// <summary>
        /// 获取微信授权的Url地址。
        /// </summary>
        /// <param name="callbackUrl">需要跳转的目标地址。</param>
        /// <param name="scope">OAuth域。</param>
        /// <param name="state">状态参数。</param>
        /// <returns>绝对的Url地址。</returns>
        public string GetAuthorizeUrl(string callbackUrl, WechatOauthScope scope, string state = null)
        {
            var uri = scope == WechatOauthScope.Login ? "https://open.weixin.qq.com/connect/qrconnect" : "https://open.weixin.qq.com/connect/oauth2/authorize";

            var redirectUrl =
                string.Format(
                    "{0}?appid={1}&redirect_uri={2}&response_type=code&scope={3}&state={4}#wechat_redirect",
                    uri, _apiConfig.AppId, System.Web.HttpUtility.UrlEncode(callbackUrl), "snsapi_" + scope.ToString().ToLower(), state);

            return redirectUrl;
        }

        /// <summary>
        /// 获取用户OpenId 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetUserOpenId(string code)
        {
            var oAuthAccessToken = this.GetAccessTokenByCode(code);
            return oAuthAccessToken?.OpenId;
        }
        /// <summary>
        /// 根据Code获取用户信息。 language zh_CN
        /// </summary>
        /// <param name="code">授权返回的code。</param>
        /// <param name="language">zh_CN 简体，zh_TW 繁体，en 英语。</param>
        /// <returns>用户信息。</returns>
        public WechatUserInfo GetUserInfo(string code, string language = "zh_CN")
        {
            var oAuthAccessToken = this.GetAccessTokenByCode(code);
            var result = _httpClient.HttpGet<WechatUserInfo>($"/sns/userinfo?access_token={oAuthAccessToken.AccessToken}&openid={oAuthAccessToken.OpenId}&lang={language}");
            if (result.IsError())
            {
                return null;
            }
            return result;
        }

        #region 获取微信jssdk配置
        /// <summary>
        /// 获取jssdk配置
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetJsSdkConfig(string url)
        {
            var accessResult = GetAccessToken();
            if (accessResult.IsError())
            {
                return new Dictionary<string, string>()
                {
                    { "error",accessResult?.Errmsg }
                };
            }
            var apiTicket = GetJsApiTicket(accessResult.AccessToken);
            if (apiTicket == null)
                return null;
            var timestamp = DateTime.Now.GetDateTimeStamp().ToString();
            var nonceStr = Guid.NewGuid().ToString("N");
            var signature = EncryptHelper.SHA1Encrypt("jsapi_ticket=" + apiTicket + "&noncestr=" + nonceStr +
                                     "&timestamp=" + timestamp + "&url=" + url);
            return new Dictionary<string, string>() {
                { "appId", _apiConfig.AppId },
                { "timestamp", timestamp },
                { "nonceStr", nonceStr },
                { "signature",signature}
            };
        }
        /// <summary>
        /// 发送微信模板消息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public WechatBaseModel SendWxMsg(object data)
        {
            var accessResult = GetAccessToken();
            if (accessResult.IsError())
            {
                return accessResult;
            }
            var paramData = JsonHelper.Serialize(data);
            var resp = _httpClient.HttpPost<WechatBaseModel>($"/cgi-bin/message/template/send?access_token={accessResult.AccessToken}", paramData);
            return resp;
        }
        private string GetJsApiTicket(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                return null;
            return _sdkHook.CacheApiTicket(accessToken, () =>
            {
                var result = _httpClient.HttpGet<JsSdkApiTicketModel>($"/cgi-bin/ticket/getticket?access_token={accessToken}&type=jsapi");
                return result;
            });
        }
        #endregion

    }
}
