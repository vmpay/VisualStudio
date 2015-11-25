using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Calc.Models
{
    public class CalcInput
    {
        [JsonProperty(PropertyName = "a")]
        public double a;

        [JsonProperty(PropertyName = "b")]
        public double b;
    }
}