using Template.Components.Security;
using System.Diagnostics.CodeAnalysis;

namespace Template.Tests.Unit.Components.Security
{
    [AllowUnauthorized]
    [ExcludeFromCodeCoverage]
    public class AllowUnauthorizedController : AuthorizedController
    {
    }
}
