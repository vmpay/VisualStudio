using Template.Components.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{
    public class ProfileDeleteView : BaseView
    {
        [Required]
        [NotTrimmed]
        [StringLength(128)]
        public String Password { get; set; }
    }
}
