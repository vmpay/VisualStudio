﻿using Template.Components.Mvc;
using Template.Resources.Form;
using Template.Tests.Objects;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Xunit;

namespace Template.Tests.Unit.Components.Mvc
{
    public class RangeAdapterTests
    {
        #region Constructor: RangeAdapter(ModelMetadata metadata, ControllerContext context, RangeAttribute attribute)

        [Fact]
        public void RangeAdapter_SetsErrorMessage()
        {
            RangeAttribute attribute = new RangeAttribute(0, 128);
            ModelMetadata metadata = new DataAnnotationsModelMetadataProvider()
                .GetMetadataForProperty(null, typeof(AdaptersModel), "Range");
            new RangeAdapter(metadata, new ControllerContext(), attribute);

            String expected = Validations.FieldMustBeInRange;
            String actual = attribute.ErrorMessage;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
