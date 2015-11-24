using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Kalkulator.Models
{
    public class Kalkul
    {
        [JsonProperty(PropertyName = "a")]
        public int a;
        [JsonProperty(PropertyName = "b")]
        public int b;
    }
}