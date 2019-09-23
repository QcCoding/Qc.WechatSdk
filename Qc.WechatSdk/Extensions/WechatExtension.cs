using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Qc.WechatSdk
{
    public static class WechatApiExtension
    {
        public static bool IsError(this WechatBaseModel input)
        {
            return input == null || string.IsNullOrEmpty(input.Errcode) == false && input.Errcode != "0";
        }
        public static bool IsSuccess(this WechatBaseModel input)
        {
            return input != null && string.IsNullOrWhiteSpace(input.Errcode) || input.Errcode == "0";
        }
    }
}
