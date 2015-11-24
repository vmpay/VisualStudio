using Template.Components.Extensions.Html;
using Template.Objects;
using Xunit;

namespace Template.Tests.Unit.Objects
{
    public class RoleViewTests
    {
        #region Constructor: RoleView()

        [Fact]
        public void RoleView_CreatesEmpty()
        {
            JsTree actual = new RoleView().PrivilegesTree;

            Assert.Empty(actual.SelectedIds);
            Assert.Empty(actual.Nodes);
        }

        #endregion
    }
}
