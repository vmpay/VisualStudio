﻿using Template.Components.Logging;
using Template.Components.Mail;
using Template.Components.Mvc;
using Template.Components.Security;
using Template.Controllers;
using Template.Data.Core;
using Template.Data.Logging;
using Template.Services;
using Template.Validators;
using Template.Web;
using Template.Web.DependencyInjection;
using System;
using System.Data.Entity;
using System.Web.Mvc;
using Xunit;
using Xunit.Extensions;

namespace Template.Tests.Unit.Web.DependencyInjection
{
    public class MainContainerTests : IDisposable
    {
        private MainContainer container;

        public MainContainerTests()
        {
            container = new MainContainer();
            container.RegisterServices();
        }
        public void Dispose()
        {
            container.Dispose();
        }

        #region Method: RegisterServices()

        [Theory]
        [InlineData(typeof(DbContext), typeof(Context))]
        [InlineData(typeof(IUnitOfWork), typeof(UnitOfWork))]

        [InlineData(typeof(ILogger), typeof(Logger))]
        [InlineData(typeof(IAuditLogger), typeof(AuditLogger))]

        [InlineData(typeof(IHasher), typeof(BCrypter))]
        [InlineData(typeof(IMailClient), typeof(SmtpMailClient))]

        [InlineData(typeof(IRouteConfig), typeof(RouteConfig))]
        [InlineData(typeof(IBundleConfig), typeof(BundleConfig))]
        [InlineData(typeof(IExceptionFilter), typeof(ExceptionFilter))]

        [InlineData(typeof(IMvcSiteMapParser), typeof(MvcSiteMapParser))]

        [InlineData(typeof(IRoleService), typeof(RoleService))]
        [InlineData(typeof(IAccountService), typeof(AccountService))]

        [InlineData(typeof(IRoleValidator), typeof(RoleValidator))]
        [InlineData(typeof(IAccountValidator), typeof(AccountValidator))]
        public void RegisterServices_Transient(Type abstraction, Type expectedType)
        {
            Object expected = container.GetInstance(abstraction);
            Object actual = container.GetInstance(abstraction);

            Assert.IsType(expectedType, actual);
            Assert.NotSame(expected, actual);
        }

        [Theory]
        [InlineData(typeof(IAuthorizationProvider), typeof(AuthorizationProvider))]
        public void RegisterServices_Singleton(Type abstraction, Type expectedType)
        {
            Object expected = container.GetInstance(abstraction);
            Object actual = container.GetInstance(abstraction);

            Assert.IsType(expectedType, actual);
            Assert.Same(expected, actual);
        }

        #endregion
    }
}
