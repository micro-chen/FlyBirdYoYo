using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FlyBirdYoYo.DomainEntity.ViewModel
{
    /// <summary>
    /// 枚举转化为键值对模型
    /// </summary>
    public class EnumKeyValueViewModel
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }


        [JsonProperty("tips")]
        public string Tips { get; set; }

    }
}
