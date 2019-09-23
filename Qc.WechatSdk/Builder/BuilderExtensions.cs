using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;

namespace Qc.WechatSdk
{
    public static class BuilderExtensions
    {
        public static IApplicationBuilder UseWechatMpSdk(this IApplicationBuilder app, Func<WechatConfig> configHandler)
        {
            return app;
        }
    }
}
