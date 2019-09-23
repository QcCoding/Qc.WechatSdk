using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Qc.WechatSdk
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加微信公众号 SDK，注入自定义实现的IWechatSdkHook
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="optionsAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddWechatSdk(this IServiceCollection services, Action<WechatConfig> optionsAction)
        {
            services.AddWechatSdk<DefaultWechatSdkHook>(optionsAction);

            return services;
        }
        /// <summary>
        /// 添加微信公众号 SDK
        /// </summary>
        /// <param name="services"></param>
        /// <param name="optionsAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddWechatSdk<T>(this IServiceCollection services, Action<WechatConfig> optionsAction = null) where T : class, IWechatSdkHook
        {
            if (optionsAction != null)
            {
                services.Configure(optionsAction);
            }
            services.AddScoped<IWechatSdkHook, T>();
            services.AddScoped<WechatService, WechatService>();
            services.AddHttpClient();
            return services;
        }
    }
}
