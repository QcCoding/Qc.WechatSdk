using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Qc.WechatSdk.Models
{
    public class JsSdkApiTicketModel : WechatBaseModel
    {
        public string Ticket { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        public DateTime? ExpiresEndTime { get; set; }
    }
}
