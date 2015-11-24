﻿using Template.Components.Mvc;
using Template.Resources.Form;
using Template.Tests.Objects;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Xunit;

namespace Template.Tests.Unit.Components.Mvc
{
    public class EmailAddressAdapterTests
    {
        private EmailAddressAttribute attribute;
        private EmailAddressAdapter adapter;
        private ModelMetadata metadata;

        public EmailAddressAdapterTests()
        {
            attribute = new EmailAddressAttribute();
            metadata = new DataAnnotationsModelMetadataProvider()
                .GetMetadataForProperty(null, typeof(AdaptersModel), "EmailAddress");
            adapter = new EmailAddressAdapter(metadata, new ControllerContext(), attribute);
        }

        #region Constructor: EmailAddressAdapter(ModelMetadata metadata, ControllerContext context, EmailAddressAttribute attribute)

        [Fact]
        public void EmailAddressAdapter_SetsErrorMessage()
        {
            String expected = Validations.FieldIsNotValidEmail;
            String actual = attribute.ErrorMessage;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: GetClientValidationRules()

        [Fact]
        public void GetClientValidationRules_ReturnsEmailValidationRule()
        {
            String expectedMessage = String.Format(Validations.FieldIsNotValidEmail, metadata.GetDisplayName());
            ModelClientValidationRule actual = adapter.GetClientValidationRules().Single();

            Assert.Equal(expectedMessage, actual.ErrorMessage);
            Assert.Equal("email", actual.ValidationType);
            Assert.Empty(actual.ValidationParameters);
        }

        #endregion
    }
}
