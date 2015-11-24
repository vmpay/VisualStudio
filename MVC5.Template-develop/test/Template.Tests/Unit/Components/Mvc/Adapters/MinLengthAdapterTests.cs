using Template.Components.Mvc;
using Template.Resources.Form;
using Template.Tests.Objects;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Xunit;

namespace Template.Tests.Unit.Components.Mvc
{
    public class MinLengthAdapterTests
    {
        #region Constructor: MinLengthAdapter(ModelMetadata metadata, ControllerContext context, MinLengthAttribute attribute)

        [Fact]
        public void MinLengthAdapter_SetsErrorMessage()
        {
            ModelMetadata metadata = new DataAnnotationsModelMetadataProvider()
                .GetMetadataForProperty(null, typeof(AdaptersModel), "MinLength");

            MinLengthAttribute attribute = new MinLengthAttribute(128);
            new MinLengthAdapter(metadata, new ControllerContext(), attribute);

            String expected = Validations.FieldMustBeWithMinLengthOf;
            String actual = attribute.ErrorMessage;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
