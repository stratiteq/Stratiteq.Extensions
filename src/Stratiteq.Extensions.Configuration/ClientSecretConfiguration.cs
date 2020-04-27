// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Stratiteq.Extensions.Configuration
{
    public class ClientSecretConfiguration : AzureADConfiguration
    {
        /// <summary>
        /// Gets the secret string that the application uses to prove its identity when requesting a token. Also can be referred to as application password.
        /// </summary>
        public string? ClientSecret { get; internal set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = base.Validate(validationContext).ToList();

            if (string.IsNullOrEmpty(this.ClientSecret))
            {
                errors.Add(new ValidationResult(string.Format(MissingAppSettingTemplate, nameof(this.ClientSecret))));
            }

            return errors;
        }
    }
}
