using System;
using System.Collections.Generic;
using System.Text;

namespace Qc.WechatSdk
{
    public class WechatBaseModel
    {
        /// <summary>
        /// 错误码 失败返回 invalid_client
        /// </summary>
        public string Errcode { get; set; }
        public string Errmsg { get; set; }
    }
}
