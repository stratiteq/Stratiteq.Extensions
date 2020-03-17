// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Stratiteq.MicroserviceTemplate.Configurations
{
    public class CosmosDBConfiguration : IValidatableObject
    {
        private const string MissingAppSettingTemplate = "Configuration not valid. App setting \"{0}\" that is required for the application to start is missing.";

        public CosmosDBConfiguration()
        {
        }

        public string? CosmosDbAccountEndpoint { get; private set; }

        public string? CosmosDbPrimaryKey { get; private set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            if (string.IsNullOrEmpty(CosmosDbAccountEndpoint))
            {
                errors.Add(new ValidationResult(string.Format(MissingAppSettingTemplate, nameof(CosmosDbAccountEndpoint))));
            }

            if (string.IsNullOrEmpty(CosmosDbPrimaryKey))
            {
                errors.Add(new ValidationResult(string.Format(MissingAppSettingTemplate, nameof(CosmosDbPrimaryKey))));
            }

            return errors;
        }
    }
}
