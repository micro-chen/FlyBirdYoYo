using System;
using System.Collections.Generic;
using System.Text;

namespace Top.Api.Util
{
    public class ApiListTypeAttribute : Attribute
    {
        public String Value { get; set; }

        public ApiListTypeAttribute(string value)
        {
            this.Value = value;
        }
    }
}
