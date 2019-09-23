using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Qc.WechatSdk.Models
{
    public class WechatAccessTokenModel: WechatBaseModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("openid")]
        public string OpenId { get; set; }
        [JsonProperty("scope")]
        public string ScopeString { get; set; }
        [JsonIgnore]
        public WechatOauthScope Scope
        {
            get
            {
                switch (ScopeString)
                {
                    case "snsapi_base":
                        return WechatOauthScope.Base;

                    case "snsapi_userinfo":
                        return WechatOauthScope.UserInfo;
                }
                return default(WechatOauthScope);
            }
        }
        public DateTime? ExpiresEndTime { get; set; }
    }
}
