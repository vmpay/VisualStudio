﻿using Template.Components.Extensions.Html;
using NSubstitute;
using System;
using System.IO;
using System.Web.Mvc;
using Xunit;

namespace Template.Tests.Unit.Components.Extensions.Html
{
    public class ContentExtensionsTests
    {
        private String path;
        private HtmlHelper html;
        private UrlHelper urlHelper;

        public ContentExtensionsTests()
        {
            path = "ContentExtensionsTests.txt";
            File.WriteAllText(path, "");

            html = HtmlHelperFactory.CreateHtmlHelper();
            urlHelper = new UrlHelper(html.ViewContext.RequestContext);
        }

        #region Extension method: RenderControllerScript(this HtmlHelper html)

        [Fact]
        public void RenderControllerScript_WithArea()
        {
            String scriptSrc = urlHelper.Content("~/Scripts/Shared/administration/accounts/accounts.js");
            html.ViewContext.HttpContext.Server.MapPath(scriptSrc).Returns(path);

            String expected = String.Format("<script src=\"{0}\"></script>", scriptSrc);
            String actual = html.RenderControllerScript().ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RenderControllerScript_WithoutArea()
        {
            String scriptSrc = urlHelper.Content("~/Scripts/Shared/accounts/accounts.js");
            html.ViewContext.HttpContext.Server.MapPath(scriptSrc).Returns(path);
            html.ViewContext.RouteData.Values["Area"] = null;

            String expected = String.Format("<script src=\"{0}\"></script>", scriptSrc);
            String actual = html.RenderControllerScript().ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RenderControllerScript_NoScripts_DoesNotRender()
        {
            html.ViewContext.HttpContext.Server.MapPath(Arg.Any<String>()).Returns("");

            String actual = html.RenderControllerScript().ToString();
            String expected = "";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: RenderControllerStyle(this HtmlHelper html)

        [Fact]
        public void RenderControllerStyle_WithArea()
        {
            String styleRef = urlHelper.Content("~/Content/Shared/administration/accounts/accounts.css");
            html.ViewContext.HttpContext.Server.MapPath(styleRef).Returns(path);

            String expected = String.Format("<link href=\"{0}\" rel=\"stylesheet\"></link>", styleRef);
            String actual = html.RenderControllerStyle().ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RenderControllerStyle_WithoutArea()
        {
            String styleRef = urlHelper.Content("~/Content/Shared/accounts/accounts.css");
            html.ViewContext.HttpContext.Server.MapPath(styleRef).Returns(path);
            html.ViewContext.RouteData.Values["Area"] = null;

            String expected = String.Format("<link href=\"{0}\" rel=\"stylesheet\"></link>", styleRef);
            String actual = html.RenderControllerStyle().ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RenderControllerStyle_NoStyles_DoesNotRender()
        {
            html.ViewContext.HttpContext.Server.MapPath(Arg.Any<String>()).Returns("");

            String actual = html.RenderControllerStyle().ToString();
            String expected = "";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
