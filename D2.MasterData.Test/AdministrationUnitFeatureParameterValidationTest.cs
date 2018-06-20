﻿using D2.Infrastructure;
using D2.MasterData.Infrastructure;
using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace D2.MasterData.Test
{
    public class AdministrationUnitFeatureParameterValidationTest
    {
        [Fact(DisplayName = "Validation of AdministrationUnitFeatureParameters succeeds for valid post")]
        public void ValidationOfAdministrationUnitFeatureParameters_succeeds_for_valid_post()
        {
            var parameters = new AdministrationUnitFeatureParameters
            {
                Title = "Wohnflaeche",
                Description = "Wird benoetigt",
                Tag = VariantTag.TypedValue,
                TypedValueDecimalPlace = 2,
                TypedValueUnit = "meter"
            };

            var validator = new ParameterValidator();

            var result = validator.Validate(parameters, RequestType.Post);
            Assert.True(result.IsValid);
        }


        [Fact(DisplayName = "Validation of AdministrationUnitFeatureParameters for post fails w/o Titel")]
        public void Validation_of_AdministrationUnitFeatureParameters_for_post_fails_wo_Titel()
        {
            var parameters = new AdministrationUnitFeatureParameters
            {
                Description = "Wird benoetigt",
                Tag = VariantTag.TypedValue
            };

            var validator = new ParameterValidator();

            var result = validator.Validate(parameters, RequestType.Post);
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
        }

        [Fact(DisplayName = "Validation of AdministrationUnitFeatureParameters for post fails with null")]
        public void Validation_of_AdministrationUnitFeatureParameters_for_post_fails_with_null()
        {
            var validator = new ParameterValidator();

            var result = validator.Validate(null, RequestType.Post);
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
        }
    }
}
