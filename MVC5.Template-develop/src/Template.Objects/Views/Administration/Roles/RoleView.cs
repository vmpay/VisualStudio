using Datalist;
using Template.Components.Extensions.Html;
using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{
    public class RoleView : BaseView
    {
        [Required]
        [DatalistColumn]
        [StringLength(128)]
        public String Title { get; set; }

        public JsTree PrivilegesTree { get; set; }

        public RoleView()
        {
            PrivilegesTree = new JsTree();
        }
    }
}
