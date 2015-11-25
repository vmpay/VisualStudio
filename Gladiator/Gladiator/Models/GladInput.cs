using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Gladiator.Models
{
    public class GladInput
    {
        //Hit points
        [JsonProperty(PropertyName = "a")]
        public double a;
        //Attack points
        [JsonProperty(PropertyName = "b")]
        public double b;
        //Critical strike chance bonus
        [JsonProperty(PropertyName = "c")]
        public double c;
        //Enemy's lvl
        [JsonProperty(PropertyName = "d")]
        public int d;
    }
}