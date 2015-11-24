using Template.Objects;
using System;

namespace Template.Validators
{
    public interface IRoleValidator : IValidator
    {
        Boolean CanCreate(RoleView view);
        Boolean CanEdit(RoleView view);
    }
}
