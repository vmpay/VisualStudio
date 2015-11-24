﻿using Template.Web;
using System;
using System.Web.Optimization;
using Xunit;

namespace Template.Tests.Unit.Web
{
    public class BundleConfigTests
    {
        private BundleConfig config;

        public BundleConfigTests()
        {
            config = new BundleConfig();
        }

        #region Method: RegisterBundles(BundleCollection bundles)

        [Fact]
        public void RegisterBundles_ForScripts()
        {
            String[] expectedBundles =
            {
                "~/Scripts/JQuery/Bundle",
                "~/Scripts/Bootstrap/Bundle",
                "~/Scripts/JQueryUI/Bundle",
                "~/Scripts/MvcGrid/Bundle",
                "~/Scripts/JsTree/Bundle",
                "~/Scripts/Datalist/Bundle",
                "~/Scripts/Shared/Bundle"
            };

            BundleCollection bundles = new BundleCollection();
            config.RegisterBundles(bundles);

            foreach (String path in expectedBundles)
                Assert.IsType<ScriptBundle>(bundles.GetBundleFor(path));
        }

        [Fact]
        public void RegisterBundles_ForStyles()
        {
            String[] expectedBundles =
            {
                "~/Content/Bootstrap/Bundle",
                "~/Content/JQueryUI/Bundle",
                "~/Content/FontAwesome/Bundle",
                "~/Content/MvcGrid/Bundle",
                "~/Content/JsTree/Bundle",
                "~/Content/Datalist/Bundle",
                "~/Content/Shared/Bundle"
            };

            BundleCollection bundles = new BundleCollection();
            config.RegisterBundles(bundles);

            foreach (String path in expectedBundles)
                Assert.IsType<StyleBundle>(bundles.GetBundleFor(path));
        }

        #endregion
    }
}
