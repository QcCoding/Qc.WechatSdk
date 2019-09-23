using System;
using System.Collections.Generic;
using System.Text;

namespace Qc.WechatSdk
{

    public class WechatConfig : WechatBaseConfig
    {
        /// <summary>
        /// 接口地址
        /// </summary>
        public string ApiUrl { get; set; } = "https://api.weixin.qq.com";
        /// <summary>
        /// 请求超时时间
        /// </summary>
        public int? Timeout { get; set; } = 30;
        /// <summary>
        /// token保存目录 默认 ./AppData
        /// </summary>
        public string SaveTokenDirPath { get; set; } = "./AppData";
    }
}
