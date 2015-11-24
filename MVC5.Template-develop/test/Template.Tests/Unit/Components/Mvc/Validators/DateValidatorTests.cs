﻿using Template.Components.Mvc;
using Template.Objects;
using Template.Resources.Form;
using System;
using System.Linq;
using System.Web.Mvc;
using Xunit;

namespace Template.Tests.Unit.Components.Mvc
{
    public class DateValidatorTests
    {
        private DateValidator validator;
        private ModelMetadata metadata;

        public DateValidatorTests()
        {
            metadata = new DisplayNameMetadataProvider().GetMetadataForProperty(null, typeof(AccountView), "Username");
            validator = new DateValidator(metadata, new ControllerContext());
        }

        #region Method: Validate(Object container)

        [Fact]
        public void Validate_ReturnsEmpty()
        {
            Assert.Empty(validator.Validate(null));
        }

        #endregion

        #region Method: GetClientValidationRules()

        [Fact]
        public void GetClientValidationRules_ReturnsDateValidationRule()
        {
            ModelClientValidationRule actual = validator.GetClientValidationRules().Single();
            ModelClientValidationRule expected = new ModelClientValidationRule
            {
                ValidationType = "date",
                ErrorMessage = String.Format(Validations.FieldMustBeDate, metadata.GetDisplayName())
            };

            Assert.Equal(expected.ValidationParameters, actual.ValidationParameters);
            Assert.Equal(expected.ValidationType, actual.ValidationType);
            Assert.Equal(expected.ErrorMessage, actual.ErrorMessage);
        }

        #endregion
    }
}
