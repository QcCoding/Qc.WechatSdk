using System;
using System.Collections.Generic;
using System.Text;

namespace Qc.WechatSdk
{
    public enum WechatOauthScope
    {
        /// <summary>
        /// 基础身份验证
        /// </summary>
        Base = 0,
        /// <summary>
        /// 授权登录
        /// </summary>
        UserInfo = 1,
        /// <summary>
        /// pc 扫码登录
        /// </summary>
        Login = 2
    }
}
