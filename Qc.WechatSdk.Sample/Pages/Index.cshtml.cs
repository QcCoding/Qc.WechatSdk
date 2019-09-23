using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Qc.WechatSdk;

namespace Qc.WechatSdk.Sample.Pages
{
    public class IndexModel : PageModel
    {
        private readonly WechatService _service;
        public IndexModel(WechatService service)
        {
            _service = service;
        }
        public IActionResult OnGet()
        {
            var actType = Request.Query["state"].ToString();
            if (actType.StartsWith("callback"))
            {
                if (actType.EndsWith("base"))
                {
                    var openId = _service.GetUserOpenId(Request.Query["code"]);
                    return new JsonResult(new
                    {
                        openId = openId
                    });
                }
                if (actType.EndsWith("userinfo"))
                {
                    var user = _service.GetUserInfo(Request.Query["code"]);
                    return new JsonResult(user);
                }
                if (actType.EndsWith("sendwxmsg"))
                {
                    var openId = _service.GetUserOpenId(Request.Query["code"]);
                    var result = _service.SendWxMsg(new
                    {
                        touser = openId,
                        template_id = "6SL5KY3jYnQmAko_RpKZzOtNaRqAfs0ef0AT3ma0fc0",
                        url = "http://weixin.qq.com/download",
                        data = new
                        {
                            first = new { value = "恭喜你购买成功！", color = "#173177" },
                            keyword1 = new { value = "巧克力", color = "#173177" },
                            keyword2 = new { value = "39.8元", color = "#173177" },
                            keyword3 = new { value = "2014年9月22日", color = "#173177" },
                            remark = new { value = "欢迎再次购买！", color = "#173177" }
                        }
                    });
                    return new JsonResult(result);
                }
            }
            return OnPostGetJsSdk();
        }
        public string RawUrl
        {
            get
            {
                var callbackUrl = new StringBuilder()
                    .Append(Request.Scheme)
                    .Append("://")
                    .Append(Request.Host)
                    .Append(Request.PathBase)
                    .Append(Request.Path)
                    .Append(Request.QueryString)
                    .ToString();
                return callbackUrl.ToString();
            }
        }
        public string SdkConfigData { get; set; }
        public IActionResult OnPostGetAccessToken()
        {
            var result = _service.GetAccessToken();
            return new JsonResult(result);
        }
        public IActionResult OnPostGetBaseInfo()
        {
            var url = _service.GetAuthorizeUrl(RawUrl, WechatOauthScope.Base, "callback_base");
            return Redirect(url);
        }
        public IActionResult OnPostGetUserInfo()
        {
            var url = _service.GetAuthorizeUrl(RawUrl, WechatOauthScope.UserInfo, "callback_userinfo");
            return Redirect(url);
        }
        public IActionResult OnPostSendMsg()
        {
            var url = _service.GetAuthorizeUrl(RawUrl, WechatOauthScope.Base, "callback_sendwxmsg");
            return Redirect(url);
        }
        public IActionResult OnPostGetJsSdk()
        {
            var url = new StringBuilder()
                .Append(Request.Scheme)
                .Append("://")
                .Append(Request.Host)
                .Append(Request.PathBase)
                .Append(Request.Path)
                .Append(Request.QueryString)
                .ToString();
            var jssdkConfig = _service.GetJsSdkConfig(url);
            SdkConfigData = Qc.WechatSdk.Utils.JsonHelper.Serialize(jssdkConfig);
            return Page();
        }
    }
}
