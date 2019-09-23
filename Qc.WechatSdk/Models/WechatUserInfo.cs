using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Qc.WechatSdk.Models
{
    public class WechatUserInfo : WechatBaseModel
    {
        /// <summary>
        /// OpenId。
        /// </summary>
        [JsonProperty("openid")]
        public string OpenId { get; set; }

        /// <summary>
        /// 昵称。
        /// </summary>
        [JsonProperty("nickname")]
        public string NickName { get; set; }

        /// <summary>
        /// 性别（0：未知，1：男，2：女）
        /// </summary>
        [JsonProperty("sex")]
        internal int Sex { get; set; }

        /// <summary>
        /// 省份。
        /// </summary>
        [JsonProperty("province")]
        public string Province { get; set; }

        /// <summary>
        /// 城市。
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        /// 国家。
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; }

        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。若用户更换头像，原有头像URL将失效。
        /// </summary>
        [JsonProperty("headimgurl")]
        public string HeadPictureUrl { get; set; }

        /// <summary>
        /// 用户特权信息，json 数组，如微信沃卡用户为（chinaunicom）。
        /// </summary>
        [JsonProperty("privilege")]
        public string[] Privilege { get; set; }

        /// <summary>
        /// UnionId。
        /// </summary>
        /// <remarks>
        /// 1、请注意，网页授权获取用户基本信息也遵循UnionID机制。即如果开发者有在多个公众号，或在公众号、移动应用之间统一用户帐号的需求，需要前往微信开放平台（open.weixin.qq.com）绑定公众号后，才可利用UnionID机制来满足上述需求。
        /// 2、UnionID机制的作用说明：如果开发者拥有多个移动应用、网站应用和公众帐号，可通过获取用户基本信息中的unionid来区分用户的唯一性，因为同一用户，对同一个微信开放平台下的不同应用（移动应用、网站应用和公众帐号），unionid是相同的。
        /// </remarks>
        [JsonProperty("unionid")]
        public string UnionId { get; set; }

    }
}
