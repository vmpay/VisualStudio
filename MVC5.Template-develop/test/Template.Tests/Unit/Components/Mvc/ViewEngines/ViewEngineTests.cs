﻿using Template.Components.Mvc;
using System;
using Xunit;

namespace Template.Tests.Unit.Components.Mvc
{
    public class ViewEngineTests
    {
        #region Constructor: ViewEngine()

        [Fact]
        public void ViewEngine_SetsAreaViewLocationFormats()
        {
            String[] expected = { "~/Views/{2}/{1}/{0}.cshtml", "~/Views/{2}/Shared/{0}.cshtml" };
            String[] actual = new ViewEngine().AreaViewLocationFormats;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ViewEngine_SetsAreaMasterLocationFormats()
        {
            String[] expected = { "~/Views/{2}/{1}/{0}.cshtml", "~/Views/{2}/Shared/{0}.cshtml" };
            String[] actual = new ViewEngine().AreaMasterLocationFormats;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ViewEngine_SetsAreaPartialViewLocationFormats()
        {
            String[] expected = { "~/Views/{2}/{1}/{0}.cshtml", "~/Views/{2}/Shared/{0}.cshtml" };
            String[] actual = new ViewEngine().AreaPartialViewLocationFormats;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
