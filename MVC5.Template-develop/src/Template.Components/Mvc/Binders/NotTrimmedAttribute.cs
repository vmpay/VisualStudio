using System;

namespace Template.Components.Mvc
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class NotTrimmedAttribute : Attribute
    {
    }
}
