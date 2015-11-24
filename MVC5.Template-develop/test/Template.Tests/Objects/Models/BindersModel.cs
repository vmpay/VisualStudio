using Template.Components.Mvc;
using System;

namespace Template.Tests.Objects
{
    public class BindersModel
    {
        [NotTrimmed]
        public String NotTrimmed { get; set; }

        public String Trimmed { get ;set; }
    }
}
