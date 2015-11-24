using Template.Components.Mvc;
using Template.Tests.Objects;
using System;
using System.Linq;
using System.Web.Mvc;
using Xunit;

namespace Template.Tests.Unit.Components.Mvc
{
    public class DigitsAdapterTests
    {
        #region Method: GetClientValidationRules()

        [Fact]
        public void GetClientValidationRules_ReturnsDigitsValidationRule()
        {
            ModelMetadata metadata = new DataAnnotationsModelMetadataProvider().GetMetadataForProperty(null, typeof(AdaptersModel), "Digits");
            DigitsAdapter adapter = new DigitsAdapter(metadata, new ControllerContext(), new DigitsAttribute());

            String expectedMessage = new DigitsAttribute().FormatErrorMessage(metadata.GetDisplayName());
            ModelClientValidationRule actual = adapter.GetClientValidationRules().Single();

            Assert.Equal(expectedMessage, actual.ErrorMessage);
            Assert.Equal("digits", actual.ValidationType);
            Assert.Empty(actual.ValidationParameters);
        }

        #endregion
    }
}
