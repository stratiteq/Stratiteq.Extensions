// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Stratiteq.Extensions.Configuration
{
    public class CosmosDBConfiguration : IValidatableObject
    {
        private const string MissingAppSettingTemplate = "Configuration not valid. App setting \"{0}\" that is required for the application to start is missing.";
        private const string ConnectionStringTemplate = "AccountEndpoint={0};AccountKey={1}";

        public CosmosDBConfiguration()
        {
        }

        public string? CosmosDbAccountEndpoint { get; private set; }

        public string? CosmosDbPrimaryKey { get; private set; }

        public string? AccountEndpoint { get; private set; }

        public string? PrimaryKey { get; private set; }

        public string? ConnectionString => string.Format(ConnectionStringTemplate, this.AccountEndpoint ?? this.CosmosDbAccountEndpoint, this.PrimaryKey ?? this.CosmosDbPrimaryKey);

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            if (string.IsNullOrEmpty(this.CosmosDbAccountEndpoint))
            {
                errors.Add(new ValidationResult(string.Format(MissingAppSettingTemplate, nameof(this.CosmosDbAccountEndpoint))));
            }

            if (string.IsNullOrEmpty(this.CosmosDbPrimaryKey))
            {
                errors.Add(new ValidationResult(string.Format(MissingAppSettingTemplate, nameof(this.CosmosDbPrimaryKey))));
            }

            return errors;
        }
    }
}
