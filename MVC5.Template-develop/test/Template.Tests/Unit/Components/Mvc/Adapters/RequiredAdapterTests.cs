using Template.Components.Mvc;
using Template.Resources.Form;
using Template.Tests.Objects;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Xunit;

namespace Template.Tests.Unit.Components.Mvc
{
    public class RequiredAdapterTests
    {
        #region Constructor: RequiredAdapter(ModelMetadata metadata, ControllerContext context, RequiredAttribute attribute)

        [Fact]
        public void RequiredAdapter_SetsErrorMessage()
        {
            ModelMetadata metadata = new DataAnnotationsModelMetadataProvider()
                .GetMetadataForProperty(null, typeof(AdaptersModel), "Required");
            RequiredAttribute attribute = new RequiredAttribute();
            new RequiredAdapter(metadata, new ControllerContext(), attribute);

            String expected = Validations.FieldIsRequired;
            String actual = attribute.ErrorMessage;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
