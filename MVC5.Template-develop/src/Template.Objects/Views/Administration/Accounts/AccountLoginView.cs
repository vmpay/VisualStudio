﻿using Template.Components.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{
    public class AccountLoginView : BaseView
    {
        [Required]
        [StringLength(128)]
        public String Username { get; set; }

        [Required]
        [NotTrimmed]
        [StringLength(128)]
        public String Password { get; set; }
    }
}
