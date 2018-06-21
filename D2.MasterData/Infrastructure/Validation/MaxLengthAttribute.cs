﻿using System;

namespace D2.MasterData.Infrastructure.Validation
{
    public class MaxLengthAttribute : ParameterValidationAttribute
    {
        private readonly int _maxLength;

        public MaxLengthAttribute(int maxLength, params RequestType[] requestTypes)
            : base(requestTypes)
        {
            _maxLength = maxLength;
        }

        public override string Error(IParameterValidator validator, object value, Type propertyType)
        {
            var text = value as string;
            if (text == null) return null;

            if (text.Length > _maxLength) return "text value contains too much characters";
            
            return null;
        }
    }
}