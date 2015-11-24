using Template.Objects;
using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Tests.Objects
{
    public class TestModel : BaseModel
    {
        [StringLength(512)]
        public String Text { get; set; }
    }
}
