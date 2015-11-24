using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Template.Objects
{
    public class Role : BaseModel
    {
        [Required]
        [StringLength(128)]
        [Index(IsUnique = true)]
        public String Title { get; set; }

        public virtual IList<RolePrivilege> RolePrivileges { get; set; }
    }
}
