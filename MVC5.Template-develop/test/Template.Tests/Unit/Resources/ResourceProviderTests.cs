﻿using Template.Objects;
using Template.Resources;
using Template.Resources.Shared;
using Template.Tests.Objects;
using System;
using System.Web.Routing;
using Xunit;
using Xunit.Extensions;

namespace Template.Tests.Unit.Resources
{
    public class ResourceProviderTests
    {
        #region Static method: GetDatalistTitle(String datalist)

        [Fact]
        public void GetDatalistTitle_IsCaseInsensitive()
        {
            String expected = Template.Resources.Datalist.Titles.Role;
            String actual = ResourceProvider.GetDatalistTitle("role");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDatalistTitle_NotFound_ReturnsNull()
        {
            Assert.Null(ResourceProvider.GetDatalistTitle("Test"));
        }

        [Fact]
        public void GetDatalistTitle_NullDatalist_ReturnsNull()
        {
            Assert.Null(ResourceProvider.GetDatalistTitle(null));
        }

        #endregion

        #region Static method: GetContentTitle(RouteValueDictionary values)

        [Fact]
        public void GetContentTitle_IsCaseInsensitive()
        {
            RouteValueDictionary values = new RouteValueDictionary();
            values["area"] = "administration";
            values["controller"] = "roles";
            values["action"] = "details";

            String expected = ContentTitles.AdministrationRolesDetails;
            String actual = ResourceProvider.GetContentTitle(values);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetContentTitle_WithoutArea(String area)
        {
            RouteValueDictionary values = new RouteValueDictionary();
            values["controller"] = "profile";
            values["action"] = "edit";
            values["area"] = area;

            String actual = ResourceProvider.GetContentTitle(values);
            String expected = ContentTitles.ProfileEdit;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetContentTitle_NotFound_ReturnsNull()
        {
            Assert.Null(ResourceProvider.GetContentTitle(new RouteValueDictionary()));
        }

        #endregion

        #region Static method: GetSiteMapTitle(String area, String controller, String action)

        [Fact]
        public void GetSiteMapTitle_IsCaseInsensitive()
        {
            String actual = ResourceProvider.GetSiteMapTitle("administration", "roles", "index");
            String expected = Template.Resources.SiteMap.Titles.AdministrationRolesIndex;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetSiteMapTitle_WithoutControllerAndAction()
        {
            String actual = ResourceProvider.GetSiteMapTitle("administration", null, null);
            String expected = Template.Resources.SiteMap.Titles.Administration;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetSiteMapTitle_NotFound_ReturnsNull()
        {
            Assert.Null(ResourceProvider.GetSiteMapTitle("Test", "Test", "Test"));
        }

        #endregion

        #region Static method: GetPrivilegeAreaTitle(String area)

        [Fact]
        public void GetPrivilegeAreaTitle_IsCaseInsensitive()
        {
            String expected = Template.Resources.Privilege.Area.Titles.Administration;
            String actual = ResourceProvider.GetPrivilegeAreaTitle("administration");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPrivilegeAreaTitle_NotFound_ReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPrivilegeAreaTitle("Test"));
        }

        [Fact]
        public void GetPrivilegeAreaTitle_NullArea_ReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPrivilegeAreaTitle(null));
        }

        #endregion

        #region Static method: GetPrivilegeControllerTitle(String area, String controller)

        [Fact]
        public void GetPrivilegeControllerTitle_ReturnsTitle()
        {
            String expected = Template.Resources.Privilege.Controller.Titles.AdministrationRoles;
            String actual = ResourceProvider.GetPrivilegeControllerTitle("Administration", "Roles");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPrivilegeControllerTitle_NotFound_ReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPrivilegeControllerTitle("", ""));
        }

        #endregion

        #region Static method: GetPrivilegeActionTitle(String area, String controller, String action)

        [Fact]
        public void GetPrivilegeActionTitle_ReturnsTitle()
        {
            String actual = ResourceProvider.GetPrivilegeActionTitle("administration", "accounts", "index");
            String expected = Template.Resources.Privilege.Action.Titles.AdministrationAccountsIndex;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPrivilegeActionTitle_NotFound_ReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPrivilegeActionTitle("", "", ""));
        }

        #endregion

        #region Static method: GetPropertyTitle<TModel, TProperty>(Expression<Func<TModel, TProperty>> property)

        [Fact]
        public void GetPropertyTitle_NotMemberExpression_ReturnNull()
        {
            Assert.Null(ResourceProvider.GetPropertyTitle<TestView, String>(view => view.ToString()));
        }

        [Fact]
        public void GetPropertyTitle_FromExpression()
        {
            String actual = ResourceProvider.GetPropertyTitle<AccountView, String>(account => account.Username);
            String expected = Template.Resources.Views.Administration.Accounts.AccountView.Titles.Username;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropertyTitle_FromExpressionRelation()
        {
            String actual = ResourceProvider.GetPropertyTitle<AccountEditView, String>(account => account.RoleId);
            String expected = Template.Resources.Views.Administration.Roles.RoleView.Titles.Id;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropertyTitle_NotFoundExpression_ReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPropertyTitle<AccountView, String>(account => account.Id));
        }

        [Fact]
        public void GetPropertyTitle_NotFoundType_ReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPropertyTitle<TestView, String>(test => test.Text));
        }

        #endregion

        #region Static method: GetPropertyTitle(Type view, String property)

        [Fact]
        public void GetPropertyTitle_IsCaseInsensitive()
        {
            String expected = Template.Resources.Views.Administration.Accounts.AccountView.Titles.Username;
            String actual = ResourceProvider.GetPropertyTitle(typeof(AccountView), "username");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropertyTitle_FromRelation()
        {
            String expected = Template.Resources.Views.Administration.Accounts.AccountView.Titles.Username;
            String actual = ResourceProvider.GetPropertyTitle(typeof(RoleView), "AccountUsername");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropertyTitle_FromMultipleRelations()
        {
            String expected = Template.Resources.Views.Administration.Accounts.AccountView.Titles.Username;
            String actual = ResourceProvider.GetPropertyTitle(typeof(RoleView), "AccountRoleAccountUsername");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropertyTitle_NotFoundProperty_ReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPropertyTitle(typeof(AccountView), "Id"));
        }

        [Fact]
        public void GetPropertyTitle_NotFoundTypeProperty_ReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPropertyTitle(typeof(TestView), "Title"));
        }

        [Fact]
        public void GetPropertyTitle_NullKey_ReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPropertyTitle(typeof(RoleView), null));
        }

        #endregion
    }
}
