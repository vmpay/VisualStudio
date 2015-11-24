﻿using Template.Components.Extensions.Html;
using Xunit;

namespace Template.Tests.Unit.Components.Extensions.Html
{
    public class JsTreeTests
    {
        #region Constructor: JsTree()

        [Fact]
        public void JsTree_CreatesEmpty()
        {
            JsTree actual = new JsTree();

            Assert.Empty(actual.Nodes);
            Assert.Empty(actual.SelectedIds);
        }

        #endregion
    }
}
