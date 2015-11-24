﻿using Template.Objects;
using Template.Resources;
using System;
using Xunit;

namespace Template.Tests.Unit.Components.Mvc
{
    public class DisplayNameMetadataProviderTests
    {
        #region Method: CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<Object> modelAccessor, Type modelType, String propertyName)

        [Fact]
        public void CreateMetadata_SetsDisplayName()
        {
            DisplayNameMetadataProviderProxy provider = new DisplayNameMetadataProviderProxy();

            String actual = provider.BaseCreateMetadata(new Attribute[0], typeof(RoleView), null, typeof(String), "Title").DisplayName;
            String expected = ResourceProvider.GetPropertyTitle(typeof(RoleView), "Title");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CreateMetadata_NullContainer_DoesNotSetDisplayName()
        {
            DisplayNameMetadataProviderProxy provider = new DisplayNameMetadataProviderProxy();

            String actual = provider.BaseCreateMetadata(new Attribute[0], null, null, typeof(String), "Name").DisplayName;

            Assert.Null(actual);
        }

        #endregion
    }
}
